using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A simple Perlin noise based low poly terrain generator
    /// </summary>
    public static class LowPolyTerrainGenerator
    {
        [Serializable]
        public class Config
        {
            public Vector3 terrainSize = new Vector3(200, 30, 200); // was 20 1 20
            public int cellSize = 1;// was 1
            public float noiseScale = 5; // was 5
            public Gradient gradient = new Gradient();
            public List<Vector3> path;
            public Vector3 offsets;
            public bool noiseArousalDirect = false;
            public bool noiseArousalInverse = false;
            public bool noiseValenceDirect = false;
            public bool noiseValenceInverse = false;
            public bool heightArousalDirect = false;
            public bool heightArousalInverse = false;
            public bool heightValenceDirect = false;
            public bool heightValenceInverse = false;
        }

        public static MeshDraft[] TerrainDraft(Config config)
        {

            Assert.IsTrue(config.terrainSize.x > 0);
            Assert.IsTrue(config.terrainSize.z > 0);
            Assert.IsTrue(config.cellSize > 0);

            var noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
            var segOffset = new Vector2(0,0);


            int xSegmentsTotal = Mathf.FloorToInt(config.terrainSize.x/config.cellSize);
            int zSegmentsTotal = Mathf.FloorToInt(config.terrainSize.z/config.cellSize);
            // need to make sure xSegTotal and zSegTotal are even because of division by 2
            // if odd, there is space between terrain parts
            xSegmentsTotal -= xSegmentsTotal % 2;
            zSegmentsTotal -= zSegmentsTotal % 2;

            int xSegments = Mathf.FloorToInt(xSegmentsTotal / 2);
            int zSegments = Mathf.FloorToInt(zSegmentsTotal / 2);

            float xStep = config.terrainSize.x/xSegmentsTotal;
            float zStep = config.terrainSize.z/zSegmentsTotal;
            int vertexCount = 6*xSegments*zSegments;

            List<Vector2> pathPoints = convertPathCoordinatesToSegments(config.path, new Vector3(xStep, zStep), config.offsets);

            MeshDraft[] drafts = new MeshDraft[4];

            for (int d = 0; d < 4; d++)
            {
                var draft = new MeshDraft
                {
                    name = "Terrain",
                    vertices = new List<Vector3>(vertexCount),
                    triangles = new List<int>(vertexCount),
                    normals = new List<Vector3>(vertexCount),
                    colors = new List<Color>(vertexCount)
                };



                for (int i = 0; i < vertexCount; i++)
                {
                    draft.vertices.Add(Vector3.zero);
                    draft.triangles.Add(0);
                    draft.normals.Add(Vector3.zero);
                    draft.colors.Add(Color.black);
                }

                for (int x = 0; x < xSegments; x++)
                {
                    for (int z = 0; z < zSegments; z++)
                    {
                        int index0 = 6 * (x + z * xSegments);
                        int index1 = index0 + 1;
                        int index2 = index0 + 2;
                        int index3 = index0 + 3;
                        int index4 = index0 + 4;
                        int index5 = index0 + 5;

                        float height00 = GetHeight(x + 0, z + 0, xSegments, zSegments, noiseOffset, config, segOffset);
                        float height01 = GetHeight(x + 0, z + 1, xSegments, zSegments, noiseOffset, config, segOffset);
                        float height10 = GetHeight(x + 1, z + 0, xSegments, zSegments, noiseOffset, config, segOffset);
                        float height11 = GetHeight(x + 1, z + 1, xSegments, zSegments, noiseOffset, config, segOffset);

                        var vertex00 = new Vector3((x + 0) * xStep, height00 * config.terrainSize.y, (z + 0) * zStep);
                        var vertex01 = new Vector3((x + 0) * xStep, height01 * config.terrainSize.y, (z + 1) * zStep);
                        var vertex10 = new Vector3((x + 1) * xStep, height10 * config.terrainSize.y, (z + 0) * zStep);
                        var vertex11 = new Vector3((x + 1) * xStep, height11 * config.terrainSize.y, (z + 1) * zStep);

                        draft.vertices[index0] = vertex00;
                        draft.vertices[index1] = vertex01;
                        draft.vertices[index2] = vertex11;
                        draft.vertices[index3] = vertex00;
                        draft.vertices[index4] = vertex11;
                        draft.vertices[index5] = vertex10;

                        draft.colors[index0] = config.gradient.Evaluate(height00);
                        draft.colors[index1] = config.gradient.Evaluate(height01);
                        draft.colors[index2] = config.gradient.Evaluate(height11);
                        draft.colors[index3] = config.gradient.Evaluate(height00);
                        draft.colors[index4] = config.gradient.Evaluate(height11);
                        draft.colors[index5] = config.gradient.Evaluate(height10);

                        Vector3 normal000111 = Vector3.Cross(vertex01 - vertex00, vertex11 - vertex00).normalized;
                        Vector3 normal001011 = Vector3.Cross(vertex11 - vertex00, vertex10 - vertex00).normalized;

                        draft.normals[index0] = normal000111;
                        draft.normals[index1] = normal000111;
                        draft.normals[index2] = normal000111;
                        draft.normals[index3] = normal001011;
                        draft.normals[index4] = normal001011;
                        draft.normals[index5] = normal001011;

                        draft.triangles[index0] = index0;
                        draft.triangles[index1] = index1;
                        draft.triangles[index2] = index2;
                        draft.triangles[index3] = index3;
                        draft.triangles[index4] = index4;
                        draft.triangles[index5] = index5;
                    }
                }

                drafts[d] = draft;
               
                // adjust noise offset for the 4 parts of the terrain to ensure continuity
                if (d % 2 == 0)
                {
                    noiseOffset.x += config.noiseScale;
                    segOffset.x = xSegments;
                }
                else if (d == 1)
                {
                    noiseOffset.y += config.noiseScale;
                    noiseOffset.x -= config.noiseScale;
                    segOffset.x = 0;
                    segOffset.y = xSegments;
                }
            }

            return drafts;
        }

        private static List<Vector2> convertPathCoordinatesToSegments(List<Vector3> points, Vector2 stepSizes, Vector3 offsets)
        {
            // if using plane we also need to scale plane -- terrain
            List<Vector2> converted = new List<Vector2>();

            foreach (Vector3 point in points)
            {
                Vector3 curr = point - offsets;
                int xSeg = Mathf.FloorToInt(curr.x / stepSizes.x);
                int zSeg = Mathf.FloorToInt(curr.z / stepSizes.y);
                converted.Add(new Vector2(xSeg, zSeg));
                converted.Add(new Vector2(xSeg + 1, zSeg));
                converted.Add(new Vector3(xSeg, zSeg + 1));
                converted.Add(new Vector3(xSeg + 1, zSeg + 1));      
            }

            return converted;
        }

        private static float GetHeight(int x, int z, int xSegments, int zSegments, Vector2 noiseOffset, Config config, Vector2 segOffset)
        {
            float xAdj = x + segOffset.x; //(but actually int)
            float zAdj = z + segOffset.y;

            
            float fixedHeight = 0;
            // if we want fixed height to be max 20 
            // we need to adjust the parameter based on the segments (max segment num)
            // since y scale is set to be 30, we need 0.03 multiplication
            // since the island is square xSegments = zSegments in this app
            float heightCoeff = 0.033f * 20 / xSegments;
           

            //TODO need to adjust light heights as well
            if (config.heightArousalDirect) {
                // increase height by z
                fixedHeight = zAdj * heightCoeff;
            }else if (config.heightArousalInverse) {
                //decrease height by z
                fixedHeight = ((zSegments * 2) - zAdj) * heightCoeff;
            } else if (config.heightValenceDirect) {
                // increase height by x
                fixedHeight = xAdj * heightCoeff;
            } else if (config.heightValenceInverse) {
                //decrease height by x
                fixedHeight = ((xSegments * 2) - xAdj) * heightCoeff;
            }

            float noiseCoeff = 1;
            if (config.noiseArousalDirect) {
                 // increase noise by z
                noiseCoeff = (zAdj / (zSegments * 2)) + (float)Math.Pow((zAdj / (zSegments * 2)), 2) * 2;
            }else if (config.noiseArousalInverse) {
                // decrease noise by z
                noiseCoeff = (((zSegments * 2) - zAdj) / (zSegments * 2)) + (float)Math.Pow((((zSegments * 2) - zAdj)/ (zSegments * 2)), 2) * 2;
            } else if (config.noiseValenceDirect) {
                // increase noise by x
                noiseCoeff = (xAdj / (xSegments * 2)) + (float)Math.Pow((xAdj / (xSegments * 2)), 2) * 2;
            } else if (config.noiseValenceInverse) {
                // decrease noise by x
                noiseCoeff = (((xSegments * 2) - xAdj) / (xSegments * 2)) + (float)Math.Pow((((xSegments * 2) - xAdj)/ (xSegments * 2)), 2) * 2;
            }

            noiseCoeff = noiseCoeff * 0.7f;

            // make island, check if the point lies outside of the island ellipse
            int cX = xSegments;
            int cZ = zSegments;
            int rX = xSegments - 1;
            int rZ = zSegments - 1;
            bool isOut = (Math.Pow((xAdj - cX), 2) / Math.Pow(rX, 2)) + (Math.Pow((zAdj - cZ), 2) / Math.Pow(rZ, 2)) > 1;

            // sample elliptical path

            // check if a point is in the drawn path, currently not implemented
            //bool isPath = pathPoints.Contains(new Vector2(xAdj, zAdj));
            bool isPath = false;

            if (isOut)
            {
                return 0;
            } else if (isPath)
            {
                Debug.Log("path");
                Debug.Log(xAdj + ", " + zAdj);
                return fixedHeight;
            } else 
            {
                float noiseX = config.noiseScale * x / xSegments + noiseOffset.x;
                float noiseZ = config.noiseScale * z / zSegments + noiseOffset.y;

                return fixedHeight + noiseCoeff * Mathf.PerlinNoise(noiseX, noiseZ);
            }
        }
    }
}
