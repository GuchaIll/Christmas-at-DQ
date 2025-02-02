using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Procedral_Grid : MonoBehaviour
{
    public int xSize, ySize;

    private Mesh mesh;

    private Vector3[] vertices;
    private void Awake() {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate () {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, y);
                yield return wait;
			}
		}

        mesh.vertices = vertices;
	}


    private void OnDrawGizmos()
    {
        if(vertices == null) return;
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}


