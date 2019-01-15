using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoader : MonoBehaviour
{
    public string fileName;

    void sample()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        IntPtr plyIntPtr = PlyLoaderDll.LoadPly(filePath);

        Mesh mesh = new Mesh();
        mesh.vertices = PlyLoaderDll.GetVertices(plyIntPtr);
        mesh.uv = PlyLoaderDll.GetUvs(plyIntPtr);
        mesh.normals = PlyLoaderDll.GetNormals(plyIntPtr);
        mesh.colors32 = PlyLoaderDll.GetColors(plyIntPtr);
        mesh.SetIndices(PlyLoaderDll.GetIndexs(plyIntPtr), MeshTopology.Triangles, 0, true);
        mesh.name = "mesh";

        GameObject go = new GameObject();
        go.name = "meshNew";
        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Unlit/Texture"));
        string textureName = PlyLoaderDll.GetTextureName(plyIntPtr);
        if (textureName.Length > 0)
        {
            string texturePath = "file://" + System.IO.Path.Combine(Application.streamingAssetsPath, textureName);
            WWW www = new WWW(texturePath);
            while (!www.isDone)
            {
            }
            mr.material.mainTexture = www.texture;
        }
        PlyLoaderDll.UnLoadPly(plyIntPtr);
    }

    // Use this for initialization
    void Start()
    {
        // sample();

        TestVertexColor();
    }

    public Shader shader;
    public string absFilePath;
    void TestVertexColor()
    {
        IntPtr plyIntPtr = PlyLoaderDll.LoadPly(absFilePath);

        Vector3[] originialVertices = PlyLoaderDll.GetVertices(plyIntPtr);
        print(originialVertices[0]);
        Mesh mesh = new Mesh();
        mesh.vertices = PlyLoaderDll.GetVertices(plyIntPtr);
        //mesh.uv = PlyLoaderDll.GetUvs(plyIntPtr);
        //mesh.normals = PlyLoaderDll.GetNormals(plyIntPtr);
        mesh.colors32 = PlyLoaderDll.GetColors(plyIntPtr);
        mesh.SetIndices(PlyLoaderDll.GetIndexs(plyIntPtr), MeshTopology.Triangles, 0, true);
        mesh.name = "mesh";
        mesh.RecalculateNormals();

        GameObject go = new GameObject();
        go.name = "meshNew";
        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        //mr.material = new Material(Shader.Find("Unlit/VertexColor"));
        mr.material = new Material(shader);

        PlyLoaderDll.UnLoadPly(plyIntPtr);
    }
}