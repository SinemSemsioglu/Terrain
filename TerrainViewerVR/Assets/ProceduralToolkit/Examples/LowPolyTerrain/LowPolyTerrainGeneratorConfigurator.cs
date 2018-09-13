using ProceduralToolkit.Examples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Configurator for LowPolyTerrainGenerator with UI and editor controls
    /// </summary>
    public class LowPolyTerrainGeneratorConfigurator : ConfiguratorBase
    {
        // for teleporting
        public MeshFilter terrainDuplicateQ1;
        public MeshFilter terrainDuplicateQ2;
        public MeshFilter terrainDuplicateQ3;
        public MeshFilter terrainDuplicateQ4;

        public MeshCollider terrainDuplicateColliderQ1;
        public MeshCollider terrainDuplicateColliderQ2;
        public MeshCollider terrainDuplicateColliderQ3;
        public MeshCollider terrainDuplicateColliderQ4;

        // actual terrain
        public MeshFilter terrainMeshFilterQ1;
        public MeshFilter terrainMeshFilterQ2;
        public MeshFilter terrainMeshFilterQ3;
        public MeshFilter terrainMeshFilterQ4;

        public MeshCollider terrainMeshColliderQ1;
        public MeshCollider terrainMeshColliderQ2;
        public MeshCollider terrainMeshColliderQ3;
        public MeshCollider terrainMeshColliderQ4;

        public bool constantSeed = false;
        public LowPolyTerrainGenerator.Config config = new LowPolyTerrainGenerator.Config();

        //TODO actually offset will be -Terrainsize.x/2, 0, -terrainsize.z/2
        public Vector3 offsets;

        private const int minXSize = 10;
        private const int maxXSize = 300;
        private const int minYSize = 1;
        private const int maxYSize = 20;
        private const int minZSize = 10;
        private const int maxZSize = 300;
        private const int minCellSize = 1;
        private const int maxCellSize = 10;
        private const int minNoiseScale = 1;
        private const int maxNoiseScale = 20;

        private Mesh terrainMeshQ1;
        private Mesh terrainMeshQ2;
        private Mesh terrainMeshQ3;
        private Mesh terrainMeshQ4;

        private void Awake()
        {
            Generate();
            config.offsets = offsets;
        }

        private void Update()
        {
            //UpdateSkybox();
        }

        public LowPolyTerrainGenerator.Config getConfig() {
            return config;
        }

        public void setConfig(LowPolyTerrainGenerator.Config _config) {
            config = _config;
        }

        public void Generate(bool randomizeConfig = true)
        {
            if (constantSeed)
            {
                Random.InitState(0);
            }

            if (randomizeConfig)
            {
                //GeneratePalette();

                //config.gradient = ColorE.Gradient(from: GetMainColorHSV(), to: GetSecondaryColorHSV());
            }
            //config.path = pathCreator.GetComponent<PathCreator>().getPath();

            var drafts = LowPolyTerrainGenerator.TerrainDraft(config);
            var draftQ1 = drafts[0];
            var draftQ2 = drafts[1];
            var draftQ3 = drafts[2];
            var draftQ4 = drafts[3];

            //draftQ1.Move(Vector3.left * config.terrainSize.x / 2 + Vector3.back * config.terrainSize.z / 2);
            draftQ1.Move(Vector3.left * config.terrainSize.x / 2 + Vector3.back * config.terrainSize.z / 2);
            draftQ2.Move( Vector3.back * config.terrainSize.z / 2);
            draftQ3.Move(Vector3.left * config.terrainSize.x / 2);
            //draftQ4.Move(Vector3.right * config.terrainSize.x / 4 + Vector3.forward * config.terrainSize.z / 2);

            AssignDraftToMeshFilter(draftQ1, terrainMeshFilterQ1, ref terrainMeshQ1);
            AssignDraftToMeshFilter(draftQ2, terrainMeshFilterQ2, ref terrainMeshQ2);
            AssignDraftToMeshFilter(draftQ3, terrainMeshFilterQ3, ref terrainMeshQ3);
            AssignDraftToMeshFilter(draftQ4, terrainMeshFilterQ4, ref terrainMeshQ4);

            AssignDraftToMeshFilter(draftQ1, terrainDuplicateQ1, ref terrainMeshQ1);
            AssignDraftToMeshFilter(draftQ2, terrainDuplicateQ2, ref terrainMeshQ2);
            AssignDraftToMeshFilter(draftQ3, terrainDuplicateQ3, ref terrainMeshQ3);
            AssignDraftToMeshFilter(draftQ4, terrainDuplicateQ4, ref terrainMeshQ4);

            terrainMeshColliderQ1.sharedMesh = terrainMeshQ1;
            terrainMeshColliderQ2.sharedMesh = terrainMeshQ2;
            terrainMeshColliderQ3.sharedMesh = terrainMeshQ3;
            terrainMeshColliderQ4.sharedMesh = terrainMeshQ4;

            terrainDuplicateColliderQ1.sharedMesh = terrainMeshQ1;
            terrainDuplicateColliderQ2.sharedMesh = terrainMeshQ2;
            terrainDuplicateColliderQ3.sharedMesh = terrainMeshQ3;
            terrainDuplicateColliderQ4.sharedMesh = terrainMeshQ4;
        }
    }
}
