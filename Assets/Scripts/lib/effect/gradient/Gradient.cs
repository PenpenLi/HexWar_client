#if UNITY_5_2 || UNITY_5_3

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace xy3d.tstd.lib.effect{

	[AddComponentMenu("UI/Effects/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		[SerializeField]
		private Color32 topColor = Color.white;

		[SerializeField]
		private Color32	bottomColor = Color.black;

#if UNITY_5_2_2 || UNITY_5_2_3 || UNITY_5_3
		public override void ModifyMesh (VertexHelper vh){

			if (!IsActive ()) {
				
				return;
			}

			List<UIVertex> list = new List<UIVertex> ();

			vh.GetUIVertexStream (list);

			ModifyVertices (list);  // calls the old ModifyVertices which was used on pre 5.2

			vh.Clear();

			vh.AddUIVertexTriangleStream (list);
		}

#endif

		public override void ModifyMesh (Mesh _mesh)
		{

			if (!IsActive ()) {

				return;
			}

			List<UIVertex> list = new List<UIVertex> ();
			using (VertexHelper vertexHelper = new VertexHelper(_mesh)) {
				vertexHelper.GetUIVertexStream (list);
			}
			
			ModifyVertices (list);  // calls the old ModifyVertices which was used on pre 5.2
			
			using (VertexHelper vertexHelper2 = new VertexHelper()) {
				vertexHelper2.AddUIVertexTriangleStream (list);
				vertexHelper2.FillMesh (_mesh);
			}
		}

		public void ModifyVertices (List<UIVertex> vertexList)
		{

			int count = vertexList.Count;

			if(count == 0){

				return;
			}

			float bottomY = vertexList [0].position.y;
			float topY = vertexList [0].position.y;
			
			for (int i = 1; i < count; i++) {
				float y = vertexList [i].position.y;
				if (y > topY) {
					topY = y;
				} else if (y < bottomY) {
					bottomY = y;
				}
			}
			
			float uiElementHeight = topY - bottomY;
			
			for (int i = 0; i < count; i++) {
				UIVertex uiVertex = vertexList [i];
				uiVertex.color = Color32.Lerp (bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
				vertexList [i] = uiVertex;
			}
		}
	}
}

#else

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace xy3d.tstd.lib.effect{

	[AddComponentMenu("UI/Effects/Gradient")]
	public class Gradient : BaseVertexEffect {
		[SerializeField]
		private Color32 topColor = Color.white;
		[SerializeField]
		private Color32 bottomColor = Color.black;
		
		public override void ModifyVertices(List<UIVertex> vertexList) {
			if (!IsActive()) {
				return;
			}
			
			int count = vertexList.Count;
			float bottomY = vertexList[0].position.y;
			float topY = vertexList[0].position.y;
			
			for (int i = 1; i < count; i++) {
				float y = vertexList[i].position.y;
				if (y > topY) {
					topY = y;
				}
				else if (y < bottomY) {
					bottomY = y;
				}
			}
			
			float uiElementHeight = topY - bottomY;
			
			for (int i = 0; i < count; i++) {
				UIVertex uiVertex = vertexList[i];
				uiVertex.color = Color32.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
				vertexList[i] = uiVertex;
			}
		}
	}
}
#endif
