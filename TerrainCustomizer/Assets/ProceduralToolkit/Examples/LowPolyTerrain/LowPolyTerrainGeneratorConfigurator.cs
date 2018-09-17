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
        public MeshFilter terrainMeshFilterQ1;
        public MeshFilter terrainMeshFilterQ2;
        public MeshFilter terrainMeshFilterQ3;
        public MeshFilter terrainMeshFilterQ4;

        public MeshCollider terrainMeshColliderQ1;
        public MeshCollider terrainMeshColliderQ2;
        public MeshCollider terrainMeshColliderQ3;
        public MeshCollider terrainMeshColliderQ4;

        public RectTransform leftPanel;
        public RectTransform rightPanel;

        public Camera topCam;
        public GameObject pathCreator;

        public ToggleGroup heightToggleGroup;
        public ToggleGroup noiseToggleGroup;

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
            
            //SetupSkyboxAndPalette();

           /* InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size X", minXSize, maxXSize, (int) config.terrainSize.x, value =>
                {
                    config.terrainSize.x = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size Y", minYSize, maxYSize, (int) config.terrainSize.y, value =>
                {
                    config.terrainSize.y = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size Z", minZSize, maxZSize, (int) config.terrainSize.z, value =>
                {
                    config.terrainSize.z = value;
                    Generate();
                });*/

            InstantiateControl<TextControl>(rightPanel)
                .Initialize("General Terrain Properties");

            InstantiateControl<SliderControl>(rightPanel)
                .Initialize("Cell size", minCellSize, maxCellSize, (int) config.cellSize, value =>
                {
                    config.cellSize = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(rightPanel)
                .Initialize("Noise scale", minNoiseScale, maxNoiseScale, (int) config.noiseScale, value =>
                {
                    config.noiseScale = value;
                    Generate();
                });

            InstantiateControl<ButtonControl>(rightPanel).Initialize("Generate", () => Generate());

            /*InstantiateControl<ButtonControl>(leftPanel).Initialize("Draw Path", () =>
            {
                Camera.main.enabled = false;
                topCam.enabled = true;
                pathCreator.SetActive(true);
            });*/

            InstantiateControl<TextControl>(leftPanel)
                .Initialize("Set how height relates to valence and arousal");

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with valence",  config.heightValenceDirect, value =>
            {
                config.heightValenceDirect = value;
                Generate();
            }, heightToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with valence",  config.heightValenceInverse, value =>
            {
                 config.heightValenceInverse = value;
                Generate();
            }, heightToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with arousal", config.heightArousalDirect, value =>
            {
                 config.heightArousalDirect = value;
                Generate();
            }, heightToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with arousal", config.heightArousalInverse, value =>
            {
                config.heightArousalInverse = value;
                Generate();
            }, heightToggleGroup);

            InstantiateControl<TextControl>(leftPanel)
                .Initialize("Set how noise relates to valence and arousal");

              InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with valence",  config.noiseValenceDirect, value =>
            {
                config.noiseValenceDirect = value;
                Generate();
            }, noiseToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with valence",  config.noiseValenceInverse, value =>
            {
                 config.noiseValenceInverse = value;
                Generate();
            }, noiseToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with arousal", config.noiseArousalDirect, value =>
            {
                 config.noiseArousalDirect = value;
                Generate();
            }, noiseToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with arousal", config.heightArousalInverse, value =>
            {
                config.noiseArousalInverse = value;
                Generate();
            }, noiseToggleGroup);

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
            Debug.Log(config);
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
            config.path = pathCreator.GetComponent<PathCreator>().getPath();

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

            terrainMeshColliderQ1.sharedMesh = terrainMeshQ1;
            terrainMeshColliderQ2.sharedMesh = terrainMeshQ2;
            terrainMeshColliderQ3.sharedMesh = terrainMeshQ3;
            terrainMeshColliderQ4.sharedMesh = terrainMeshQ4;

        }
    }
}
