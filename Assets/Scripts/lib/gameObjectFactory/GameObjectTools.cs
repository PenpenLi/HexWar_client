using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using xy3d.tstd.lib.publicTools;
using xy3d.tstd.lib.modelEffect;

namespace xy3d.tstd.lib.gameObjectFactory{

	public class GameObjectTools{

		private static List<Transform> getTransforms(GameObject _obj){
			
			List<Transform> list = new List<Transform> ();

			list.Add(_obj.transform);

			MeshFilter mesh = _obj.GetComponent<MeshFilter>();

			if(mesh != null){

				GameObject.DestroyImmediate (_obj.GetComponent<MeshFilter>());
				GameObject.DestroyImmediate (_obj.GetComponent<MeshRenderer>());
			}
			
			for (int i = 0; i < _obj.transform.childCount; i++) {
				
				GameObject obj = _obj.transform.GetChild(i).gameObject;

				List<Transform> tmpList = getTransforms(obj);
				
				foreach(Transform transform in tmpList){
					
					list.Add(transform);
				}
			}
			
			return list;
		}

//		private static List<Transform> getTransforms(GameObject _obj){
//			
//			List<Transform> list = new List<Transform> ();
//			
//			for (int i = 0; i < _obj.transform.childCount; i++) {
//				
//				GameObject obj = _obj.transform.GetChild(i).gameObject;
//				
//				MeshFilter mesh = obj.GetComponent<MeshFilter>();
//				
//				if(mesh == null){
//					
//					list.Add(obj.transform);
//					
//					List<Transform> tmpList = getTransforms(obj);
//					
//					foreach(Transform transform in tmpList){
//						
//						list.Add(transform);
//					}
//					
//				}else{
//					
//					GameObject.DestroyImmediate (obj.GetComponent<MeshFilter>());
//					GameObject.DestroyImmediate (obj.GetComponent<MeshRenderer>());
//				}
//			}
//			
//			return list;
//		}
		
		public static void CombineMeshs(ref GameObject _skeleton,List<GameObject> _parts,List<GameObject> _replaceParts,RuntimeAnimatorController _animatorController,float _outline,out Mesh _resultMesh,out Material _resultMaterial,bool _addCollider,bool _fixNormals){
			
			List<Transform> transforms = getTransforms (_skeleton);

			_resultMesh = new Mesh ();
			
			List<Transform> bones = new List<Transform> ();
			List<Matrix4x4> bindposes = new List<Matrix4x4>();
			
			List<Vector3> vertices = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			List<Vector2> uv2s = new List<Vector2>();
			
			
			List<BoneWeight> boneWeights = new List<BoneWeight>();
			List<int> triangles = new List<int> ();
			
			int nowVertexCount = 0;
			
			for (int i = 0; i < _parts.Count; i++) {
				
				GameObject part = _parts[i];
				
				SkinnedMeshRenderer partRenderer = part.GetComponent<SkinnedMeshRenderer>();
				
				Mesh partMesh = partRenderer.sharedMesh;
				
				for(int m = 0 ; m < partRenderer.bones.Length ; m++){
					
					int boneIndex = -1;
					
					for(int n = 0 ; n < bones.Count ; n++){
						
						if(bones[n].name == partRenderer.bones[m].name){
							
							boneIndex = n;
							
							break;
						}
					}
					
					if(boneIndex == -1){
						
						for(int n = 0 ; n < transforms.Count ; n++){
							
							if(transforms[n].name == partRenderer.bones[m].name){
								
								bones.Add(transforms[n]);

								bindposes.Add(partRenderer.sharedMesh.bindposes[m]);
								
								break;
							}
						}
					}
				}

				Vector3[] fixNormals;
				
				if(_fixNormals){
					
					fixNormals = GetNormals(partMesh.vertices,partMesh.triangles);
					
				}else{
					
					fixNormals = partMesh.normals;
				}
				
				for(int m = 0 ; m < partMesh.vertexCount ; m++){
					
					vertices.Add(partMesh.vertices[m]);
					normals.Add(fixNormals[m]);
					uvs.Add(partMesh.uv[m]);
					uv2s.Add(new Vector2());
					
					int boneIndex0 = partMesh.boneWeights[m].boneIndex0;
					int boneIndex1 = partMesh.boneWeights[m].boneIndex1;
					
					float boneWeight0 = partMesh.boneWeights[m].weight0;
					float boneWeight1 = partMesh.boneWeights[m].weight1;
					
					string boneName0 = partRenderer.bones[boneIndex0].name;
					string boneName1 = partRenderer.bones[boneIndex1].name;
					
					for(int n = 0 ; n < bones.Count ; n++){
						
						if(bones[n].name == boneName0){
							
							boneIndex0 = n;
							
						}else if(bones[n].name == boneName1){
							
							boneIndex1 = n;
						}
					}
					
					BoneWeight boneWeight = new BoneWeight();
					
					boneWeight.boneIndex0 = boneIndex0;
					boneWeight.boneIndex1 = boneIndex1;
					
					boneWeight.weight0 = boneWeight0;
					boneWeight.weight1 = boneWeight1;
					
					boneWeights.Add(boneWeight);
				}
				
				for(int m = 0 ; m < partMesh.triangles.Length ; m++){
					
					triangles.Add(nowVertexCount + partMesh.triangles[m]);
				}
				
				nowVertexCount = nowVertexCount + partMesh.vertexCount;
			}
			
			if(_replaceParts != null){
				
				for (int i = 0; i < _replaceParts.Count; i++) {
					
					GameObject part = _replaceParts[i];
					
					SkinnedMeshRenderer partRenderer = part.GetComponent<SkinnedMeshRenderer>();
					
					Mesh partMesh = partRenderer.sharedMesh;
					
					for(int m = 0 ; m < partRenderer.bones.Length ; m++){
						
						int boneIndex = -1;
						
						for(int n = 0 ; n < bones.Count ; n++){
							
							if(bones[n].name == partRenderer.bones[m].name){
								
								boneIndex = n;
								
								break;
							}
						}
						
						if(boneIndex == -1){
							
							for(int n = 0 ; n < transforms.Count ; n++){
								
								if(transforms[n].name == partRenderer.bones[m].name){
									
									bones.Add(transforms[n]);

									bindposes.Add(partRenderer.sharedMesh.bindposes[m]);
									
									break;
								}
							}
						}
					}

					Vector3[] fixNormals;

					if(_fixNormals){

						fixNormals = GetNormals(partMesh.vertices,partMesh.triangles);

					}else{

						fixNormals = partMesh.normals;
					}
					
					for(int m = 0 ; m < partMesh.vertexCount ; m++){
						
						vertices.Add(partMesh.vertices[m]);
						normals.Add(fixNormals[m]);
						uvs.Add(partMesh.uv[m]);
						uv2s.Add(new Vector2(i + 1,0));
						
						int boneIndex0 = partMesh.boneWeights[m].boneIndex0;
						int boneIndex1 = partMesh.boneWeights[m].boneIndex1;
						
						float boneWeight0 = partMesh.boneWeights[m].weight0;
						float boneWeight1 = partMesh.boneWeights[m].weight1;
						
						string boneName0 = partRenderer.bones[boneIndex0].name;
						string boneName1 = partRenderer.bones[boneIndex1].name;
						
						for(int n = 0 ; n < bones.Count ; n++){
							
							if(bones[n].name == boneName0){
								
								boneIndex0 = n;
								
							}else if(bones[n].name == boneName1){
								
								boneIndex1 = n;
							}
						}
						
						BoneWeight boneWeight = new BoneWeight();
						
						boneWeight.boneIndex0 = boneIndex0;
						boneWeight.boneIndex1 = boneIndex1;
						
						boneWeight.weight0 = boneWeight0;
						boneWeight.weight1 = boneWeight1;
						
						boneWeights.Add(boneWeight);
					}
					
					for(int m = 0 ; m < partMesh.triangles.Length ; m++){
						
						triangles.Add(nowVertexCount + partMesh.triangles[m]);
					}
					
					nowVertexCount = nowVertexCount + partMesh.vertexCount;
				}
			}
			
			_resultMesh.vertices = vertices.ToArray();
			_resultMesh.normals = normals.ToArray();
			_resultMesh.uv = uvs.ToArray();
			_resultMesh.uv2 = uv2s.ToArray();
			
			_resultMesh.triangles = triangles.ToArray ();

			_resultMesh.bindposes = bindposes.ToArray();
			_resultMesh.boneWeights = boneWeights.ToArray();
			
			if (_outline > 0) {
				
				int oldVertexCount = _resultMesh.vertexCount;
				int newVertexCount = _resultMesh.vertexCount * 2;
				
				Vector3[] outlineVertex = new Vector3[newVertexCount];
				Vector3[] outlineNormal = new Vector3[newVertexCount];
				Vector2[] outlineUv = new Vector2[newVertexCount];
				Vector2[] outlineUv2 = new Vector2[newVertexCount];
				BoneWeight[] outlineBoneWeights = new BoneWeight[newVertexCount];
				
				for(int m = 0 ; m < oldVertexCount ; m++){
					
					Vector3 vertex = new Vector3(_resultMesh.vertices[m].x,_resultMesh.vertices[m].y,_resultMesh.vertices[m].z);
					Vector3 normal = new Vector3(_resultMesh.normals[m].x,_resultMesh.normals[m].y,_resultMesh.normals[m].z);
					
					normal.Normalize();
					
					vertex = vertex + normal * _outline;
					
					outlineVertex[m] = _resultMesh.vertices[m];
					outlineVertex[m + oldVertexCount] = vertex;
					
					outlineNormal[m] = _resultMesh.normals[m];
					outlineNormal[m + oldVertexCount] = new Vector3();
					
					outlineUv[m] = _resultMesh.uv[m];
					outlineUv[m + oldVertexCount] = new Vector2();
					
					outlineUv2[m] = _resultMesh.uv2[m];
					outlineUv2[m + oldVertexCount] = new Vector2(_resultMesh.uv2[m].x,1);
					
					outlineBoneWeights[m] = _resultMesh.boneWeights[m];
					outlineBoneWeights[m + oldVertexCount] = _resultMesh.boneWeights[m];
				}
				
				int[] outlineTriangles = new int[_resultMesh.triangles.Length * 2];
				
				for(int m = 0 ; m < _resultMesh.triangles.Length / 3 ; m++){
					
					outlineTriangles[m * 3] = _resultMesh.triangles[m * 3] + oldVertexCount;
					outlineTriangles[m * 3 + 1] = _resultMesh.triangles[m * 3 + 2] + oldVertexCount;
					outlineTriangles[m * 3 + 2] = _resultMesh.triangles[m * 3 + 1] + oldVertexCount;
					
					outlineTriangles[m * 3 + _resultMesh.triangles.Length] = _resultMesh.triangles[m * 3];
					outlineTriangles[m * 3 + 1 + _resultMesh.triangles.Length] = _resultMesh.triangles[m * 3 + 1];
					outlineTriangles[m * 3 + 2 + _resultMesh.triangles.Length] = _resultMesh.triangles[m * 3 + 2];
				}
				
				_resultMesh.vertices = outlineVertex;
				_resultMesh.normals = outlineNormal;
				_resultMesh.uv = outlineUv;
				_resultMesh.uv2 = outlineUv2;
				
				_resultMesh.triangles = outlineTriangles;
				_resultMesh.boneWeights = outlineBoneWeights;
			}
			
			GameObject obj = new GameObject ();
			
			obj.AddComponent<SkinnedMeshRenderer> ();
			
			SkinnedMeshRenderer renderer = obj.GetComponent<SkinnedMeshRenderer>();
			
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			
			renderer.receiveShadows = false;
			
			renderer.useLightProbes = false;
			
			renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			
			_resultMaterial = new Material (Shader.Find ("Custom/Hero"));
			
			_resultMaterial.mainTexture = _parts [0].GetComponent<SkinnedMeshRenderer> ().sharedMaterial.mainTexture;

//			_resultMaterial.SetInt("_PartIndex",1);
			
			renderer.sharedMaterial = _resultMaterial;
			
			renderer.bones = bones.ToArray();
			
			renderer.sharedMesh = _resultMesh;
			
			Animator animator = _skeleton.GetComponent<Animator> ();
			
			if (animator == null) {
				
				GameObject result = new GameObject ();
				
				_skeleton.transform.parent = result.transform;
				
				_skeleton = result;
				
				_skeleton.AddComponent<Animator> ();
			}
			
			obj.transform.parent = _skeleton.transform;
			
			_skeleton.GetComponent<Animator> ().runtimeAnimatorController = _animatorController;
			
			_skeleton.GetComponent<Animator> ().avatar = null;

			if(_addCollider){

				obj.AddComponent<BoxCollider>();
			}
		}
		
		public static void MeshAddOutline(GameObject _obj,Texture _lightningTexture,float _outline,out Mesh _resultMesh,out GameObject _resultObj,out Material _resultMaterial,bool _fixNormals){
			
			MeshFilter meshFilter = _obj.GetComponent<MeshFilter> ();
			
			Mesh mesh = meshFilter.sharedMesh;

			_resultMesh = new Mesh ();

			int oldVertexCount = mesh.vertexCount;

			if(_outline > 0){

				int newVertexCount = mesh.vertexCount * 2;
				
				Vector3[] outlineVertex = new Vector3[newVertexCount];
				Vector3[] outlineNormal = new Vector3[newVertexCount];
				Vector2[] outlineUv = new Vector2[newVertexCount];
				Vector2[] outlineUv2 = new Vector2[newVertexCount];
				
				Vector3[] normals;
				
				if(_fixNormals){
					
					normals = GetNormals(outlineVertex,mesh.triangles);
					
				}else{
					
					normals = mesh.normals;
				}
				
				for(int m = 0 ; m < oldVertexCount ; m++){
					
					Vector3 vertex = new Vector3(mesh.vertices[m].x,mesh.vertices[m].y,mesh.vertices[m].z);
					Vector3 normal = new Vector3(normals[m].x,normals[m].y,normals[m].z);
					
					normal.Normalize();
					
					Vector3 fixedVertex = vertex + normal * _outline;
					
					outlineVertex[m] = vertex;
					outlineVertex[m + oldVertexCount] = fixedVertex;
					
					outlineNormal[m] = normal;
					outlineNormal[m + oldVertexCount] = new Vector3();
					
					outlineUv[m] = mesh.uv[m];
					outlineUv[m + oldVertexCount] = new Vector2();
					
					outlineUv2[m] = new Vector2();
					outlineUv2[m + oldVertexCount] = new Vector2(0,1);
				}
				
				int[] outlineTriangles = new int[mesh.triangles.Length * 2];
				
				for(int m = 0 ; m < mesh.triangles.Length / 3 ; m++){
					
					outlineTriangles[m * 3] = mesh.triangles[m * 3] + oldVertexCount;
					outlineTriangles[m * 3 + 1] = mesh.triangles[m * 3 + 2] + oldVertexCount;
					outlineTriangles[m * 3 + 2] = mesh.triangles[m * 3 + 1] + oldVertexCount;
					
					outlineTriangles[m * 3 + mesh.triangles.Length] = mesh.triangles[m * 3];
					outlineTriangles[m * 3 + 1 + mesh.triangles.Length] = mesh.triangles[m * 3 + 1];
					outlineTriangles[m * 3 + 2 + mesh.triangles.Length] = mesh.triangles[m * 3 + 2];
				}
			
				_resultMesh.vertices = outlineVertex;
				_resultMesh.normals = outlineNormal;
				_resultMesh.uv = outlineUv;
				_resultMesh.uv2 = outlineUv2;
			
				_resultMesh.triangles = outlineTriangles;

			}else{

				Vector3[] outlineVertex = new Vector3[oldVertexCount];
				Vector3[] outlineNormal = new Vector3[oldVertexCount];
				Vector2[] outlineUv = new Vector2[oldVertexCount];
				Vector2[] outlineUv2 = new Vector2[oldVertexCount];

				Vector3[] normals;
				
				if(_fixNormals){
					
					normals = GetNormals(outlineVertex,mesh.triangles);
					
				}else{
					
					normals = mesh.normals;
				}

				for(int m = 0 ; m < oldVertexCount ; m++){
					
					Vector3 vertex = new Vector3(mesh.vertices[m].x,mesh.vertices[m].y,mesh.vertices[m].z);
					Vector3 normal = new Vector3(normals[m].x,normals[m].y,normals[m].z);
					
					normal.Normalize();

					outlineVertex[m] = vertex;
					
					outlineNormal[m] = normal;
					
					outlineUv[m] = mesh.uv[m];
					
					outlineUv2[m] = new Vector2();
				}

				int[] outlineTriangles = new int[mesh.triangles.Length];

				for(int m = 0 ; m < mesh.triangles.Length / 3 ; m++){

					outlineTriangles[m * 3] = mesh.triangles[m * 3];
					outlineTriangles[m * 3 + 1] = mesh.triangles[m * 3 + 1];
					outlineTriangles[m * 3 + 2] = mesh.triangles[m * 3 + 2];
				}

				_resultMesh.vertices = outlineVertex;
				_resultMesh.normals = outlineNormal;
				_resultMesh.uv = outlineUv;

				_resultMesh.uv2 = outlineUv2;

				_resultMesh.triangles = outlineTriangles;
			}

			_resultObj = new GameObject ();
			
			if(_lightningTexture == null){
			
				_resultMaterial = new Material (Shader.Find ("Custom/Weapon"));
				
				_resultMaterial.mainTexture = _obj.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture;
			
			}else{

				_resultMaterial = new Material(Shader.Find("Custom/WeaponWithLightningMove"));

				_resultMaterial.mainTexture = _obj.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture;

				_resultMaterial.SetTexture("_LightningTex",_lightningTexture);

				_resultObj.AddComponent<ModelLightningMove>();
			}
			
			_resultObj.AddComponent<MeshFilter>();
			
			_resultObj.GetComponent<MeshFilter> ().sharedMesh = _resultMesh;
			
			_resultObj.AddComponent<MeshRenderer> ();
			
			_resultObj.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			
			_resultObj.GetComponent<MeshRenderer> ().receiveShadows = false;
			
			_resultObj.GetComponent<MeshRenderer> ().useLightProbes = false;
			
			_resultObj.GetComponent<MeshRenderer> ().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			
			_resultObj.GetComponent<MeshRenderer> ().material = _resultMaterial;
		}
		
		private static Rect[] COMBINE_PART_TEXTURE_RECT_ARR = new Rect[]{new Rect (384, 128, 128, 64),new Rect (384, 192, 128, 64)};
		private static Vector4[] COMBINE_PART_UV_FIX_ARR = new Vector4[]{new Vector4 (0.25f, 0.75f, 0.25f, 0.25f),new Vector4 (0.25f, 0.75f, 0.25f, 0.0f)};
		
		public static void CombinePart(GameObject _main,GameObject[] _parts,string[] _jointNames,float[] _partScales){
			
			SkinnedMeshRenderer skinnedMeshRenderer = _main.GetComponentInChildren<SkinnedMeshRenderer>();
			
			Mesh oldMesh = skinnedMeshRenderer.sharedMesh;
			
			RenderTexture resultTexture = new RenderTexture (512, 256, 0);
			
			Graphics.SetRenderTarget (resultTexture);
			
			GL.LoadPixelMatrix(0, 512, 256,0);
			
			Graphics.DrawTexture (new Rect (0, 0, 512, 256), skinnedMeshRenderer.sharedMaterial.mainTexture);
			
			//			Graphics.SetRenderTarget (null);
			
			Vector3[] vertices = oldMesh.vertices;
			Vector3[] normals = oldMesh.normals;
			Vector2[] uvs = oldMesh.uv;
			Vector2[] uv2s = oldMesh.uv2;
			BoneWeight[] boneWeights = oldMesh.boneWeights;
			int[] triangles = oldMesh.triangles;
			
			int nowVertexCount = oldMesh.vertexCount;
			
			List<Texture> partTextureList = new List<Texture> ();
			
			for (int i = 0; i < _parts.Length; i++) {
				
				int uvFixIndex = -1;
				
				for(int m = 0 ; m < partTextureList.Count ; m++){
					
					if(_parts[i].GetComponent<MeshRenderer> ().sharedMaterial.mainTexture == partTextureList[m]){
						
						uvFixIndex = m;
						
						break;
					}
				}
				
				if(uvFixIndex == -1){
					
					uvFixIndex = partTextureList.Count;
					
//					MeshRenderer mmm = _parts[i].GetComponent<MeshRenderer> ();
					
					partTextureList.Add(_parts[i].GetComponent<MeshRenderer> ().sharedMaterial.mainTexture);
					
					Graphics.DrawTexture (COMBINE_PART_TEXTURE_RECT_ARR[uvFixIndex], _parts[i].GetComponent<MeshRenderer> ().sharedMaterial.mainTexture);
				}
				
				Mesh partMesh = _parts[i].GetComponent<MeshFilter>().sharedMesh;
				
				GameObject dummy = PublicTools.FindChild(_main,_jointNames[i]);
				
				int boneIndex = -1;
				
				for(int n = 0 ; n < skinnedMeshRenderer.bones.Length ; n++){
					
					if(skinnedMeshRenderer.bones[n].name == dummy.transform.parent.name){
						
						boneIndex = n;
						
						break;
					}
				}
				
				Array.Resize<Vector3>(ref vertices,nowVertexCount + partMesh.vertexCount);
				Array.Resize<Vector3>(ref normals,nowVertexCount + partMesh.vertexCount);
				
				Array.Resize<Vector2>(ref uvs,nowVertexCount + partMesh.vertexCount);
				
				Array.Resize<Vector2>(ref uv2s,nowVertexCount + partMesh.vertexCount);
				
//				if(partMesh.uv2.Length > 0){
//					
//					Array.ConstrainedCopy(partMesh.uv2,0,uv2s,nowVertexCount,partMesh.vertexCount);
//				}
				
				Array.Resize<BoneWeight>(ref boneWeights,nowVertexCount + partMesh.vertexCount);
				
				Matrix4x4 mm = Matrix4x4.Scale(new Vector3(_partScales[i],_partScales[i],_partScales[i]));
				
				
				
				Matrix4x4 dummyBindposes = skinnedMeshRenderer.sharedMesh.bindposes[boneIndex].inverse * (dummy.transform.worldToLocalMatrix * dummy.transform.parent.localToWorldMatrix).inverse * mm;
				
				for(int m = 0 ; m < partMesh.vertexCount ; m++){
					
					int index = m + nowVertexCount;
					
					vertices[index] = dummyBindposes.MultiplyPoint3x4(partMesh.vertices[m]);
					
					normals[index] = dummyBindposes.MultiplyVector(partMesh.normals[m]);
					
					uvs[index] = new Vector2(partMesh.uv[m].x * COMBINE_PART_UV_FIX_ARR[uvFixIndex].x + COMBINE_PART_UV_FIX_ARR[uvFixIndex].y,partMesh.uv[m].y * COMBINE_PART_UV_FIX_ARR[uvFixIndex].z + COMBINE_PART_UV_FIX_ARR[uvFixIndex].w);
					
//					if(partMesh.uv2.Length == 0){
						
						uv2s[index] = new Vector2(-1,0);

//					}else{
//
//						uv2s[index] = new Vector2(-1,partMesh.uv2[m].y);
//					}
					
					BoneWeight boneWeight = new BoneWeight();
					
					boneWeight.boneIndex0 = boneIndex;
					
					boneWeight.weight0 = 1;
					
					boneWeights[index] = boneWeight;
				}
				
				int trianglesNum = triangles.Length;
				
				Array.Resize<int>(ref triangles,trianglesNum + partMesh.triangles.Length);
				
				for(int m = 0 ; m < partMesh.triangles.Length ; m++){
					
					triangles[m + trianglesNum] = nowVertexCount + partMesh.triangles[m];
				}
				
				nowVertexCount = nowVertexCount + partMesh.vertexCount;
			}
			
			Mesh resultMesh = new Mesh ();
			
			resultMesh.bindposes = oldMesh.bindposes;
			
			resultMesh.vertices = vertices;
			resultMesh.normals = normals;
			resultMesh.uv = uvs;
			resultMesh.uv2 = uv2s;
			
			resultMesh.triangles = triangles;
			
			resultMesh.boneWeights = boneWeights;
			
			skinnedMeshRenderer.sharedMesh = resultMesh;
			
			skinnedMeshRenderer.material.mainTexture = resultTexture;
			
//			skinnedMeshRenderer.material.SetInt ("_PartIndex", 1);
		}

		public static Vector3[] GetNormals(Vector3[] _vertex,int[] _index){

			Vector3[] normal = new Vector3[_vertex.Length];

			Vector3[] faceNormal = new Vector3[_index.Length / 3];
			
			float[] areaVec = new float[_index.Length / 3];

			Dictionary<int,int> sameVertexDic = new Dictionary<int, int>();//key是顶点序号 value是顶点序号 key序号的顶点坐标和value序号的顶点坐标相同
			Dictionary<int,List<int>> faceVertex = new Dictionary<int, List<int>>();//key是顶点序号 value是面序号数组 key序号的顶点在计算法线时需要考虑value中对应序号的面

			for(int i = 0 ; i < _index.Length ; i++){
				
				if(sameVertexDic.ContainsKey(_index[i])){
					
					faceVertex[sameVertexDic[_index[i]]].Add(i / 3);
					
					continue;
				}
				
				if(faceVertex.ContainsKey(_index[i])){
					
					faceVertex[_index[i]].Add(i / 3);
					
					continue;
				}

				bool needContinue = false;
				
				for(int m = 0 ; m < _index[i] ; m++){
					
					if(_vertex[_index[i]] == _vertex[m]){
						
						sameVertexDic[_index[i]] = m;
						
						faceVertex[m].Add(i / 3);

						needContinue = true;

						break;
					}
				}

				if(needContinue){

					continue;
				}
				
				faceVertex[_index[i]] = new List<int>();
				
				faceVertex[_index[i]].Add(i / 3);
			}
			
			Vector3 p0 = new Vector3();
			Vector3 p1 = new Vector3();
			Vector3 p2 = new Vector3();
			
			float c0;
			float c1;
			float c2;
			
			float zc;
			
			for(int i = 0 ; i < _index.Length / 3 ; i++){
				
				p0.x = _vertex[_index[i * 3]].x;
				p0.y = _vertex[_index[i * 3]].y;
				p0.z = _vertex[_index[i * 3]].z;
				
				p1.x = _vertex[_index[i * 3 + 1]].x;
				p1.y = _vertex[_index[i * 3 + 1]].y;
				p1.z = _vertex[_index[i * 3 + 1]].z;
				
				p2.x = _vertex[_index[i * 3 + 2]].x;
				p2.y = _vertex[_index[i * 3 + 2]].y;
				p2.z = _vertex[_index[i * 3 + 2]].z;
				
				c0 = Vector3.Distance(p0,p1);
				c1 = Vector3.Distance(p1,p2);
				c2 = Vector3.Distance(p0,p2);
				
				zc = (c0 + c1 + c2) * 0.5f;
				
				areaVec[i] = (float)Math.Sqrt(zc * (zc - c0) * (zc - c1) * (zc - c2));

				p0 = p0 - p2;
				p1 = p1 - p2;

				p2 = Vector3.Cross(p0,p1);

//				p2 = p0.crossProduct(p1);

				p2 = Vector3.Normalize(p2);

				faceNormal[i] = p2;
			}
			
			for(int i = 0 ; i < _vertex.Length ; i++){
				
				if(sameVertexDic.ContainsKey(i)){

					normal[i] = normal[sameVertexDic[i]];

				}else{
					
					float area = 0;
					
					List<int> arr = faceVertex[i];
					
					for(int m = 0 ; m < arr.Count ; m++){

						bool needContinue = false;
						
						for(int p = 0 ; p < m ; p++){
							
							if(faceNormal[arr[m]] == faceNormal[arr[p]]){
								
								needContinue = true;

								break;
							}
						}

						if(needContinue){

							continue;
						}
						
						area = area + areaVec[arr[m]];
						
						normal[i].x = normal[i].x + faceNormal[arr[m]].x * areaVec[arr[m]];
						normal[i].y = normal[i].y + faceNormal[arr[m]].y * areaVec[arr[m]];
						normal[i].z = normal[i].z + faceNormal[arr[m]].z * areaVec[arr[m]];
					}
					
					p0.x = normal[i].x / area;
					p0.y = normal[i].y / area;
					p0.z = normal[i].z / area;

					p0 = Vector3.Normalize(p0);

					normal[i].x = p0.x;
					normal[i].y = p0.y;
					normal[i].z = p0.z;
				}
			}
			
			return normal;
		}
	}
}
