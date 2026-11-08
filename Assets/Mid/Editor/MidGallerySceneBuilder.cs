using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MidGallerySceneBuilder
{
    private const string MidRoot = "Assets/Mid";
    private const string MaterialRoot = MidRoot + "/Materials";
    private const string ScenePath = MidRoot + "/Mid_Gallery_MoonlitAltar.unity";
    private const string PavilionSourcePath =
        "Assets/S15/S15_Landscape.unity";
    private const string PavilionScenePath =
        MidRoot + "/Mid_Gallery_Pavilion.unity";

    [MenuItem("CG/Mid/Build Standalone Moonlit Altar Scene")]
    public static void BuildScene()
    {
        EnsureFolder("Assets", "Mid");
        EnsureFolder(MidRoot, "Materials");

        Material groundMaterial = CreateMaterial(
            MaterialRoot + "/Mat_Mid_Ground.mat",
            new Color(0.07f, 0.09f, 0.13f, 1f),
            0.05f,
            0.18f
        );

        Material altarMaterial = CreateMaterial(
            MaterialRoot + "/Mat_Mid_Altar.mat",
            new Color(0.12f, 0.32f, 0.36f, 1f),
            0.35f,
            0.50f
        );

        Material pillarMaterial = CreateMaterial(
            MaterialRoot + "/Mat_Mid_Pillar.mat",
            new Color(0.14f, 0.16f, 0.22f, 1f),
            0.15f,
            0.25f
        );

        Material glowMaterial = CreateEmissionMaterial(
            MaterialRoot + "/Mat_Mid_GlowOrb.mat",
            new Color(0.18f, 0.75f, 1.0f, 1f),
            4.5f
        );

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject galleryRoot = new GameObject("MidGallery_MoonlitAltar");

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.SetParent(galleryRoot.transform);
        ground.transform.position = new Vector3(0f, -0.03f, 0f);
        ground.transform.localScale = new Vector3(1.5f, 1f, 1.5f);
        ground.GetComponent<MeshRenderer>().sharedMaterial = groundMaterial;

        GameObject altar = new GameObject("ProceduralAltar_LoopGenerated");
        altar.transform.SetParent(galleryRoot.transform);
        altar.AddComponent<Mid_ProceduralAltar>();
        altar.GetComponent<MeshRenderer>().sharedMaterial = altarMaterial;

        GameObject pillars = new GameObject("Pillars_TransformedCubes");
        pillars.transform.SetParent(galleryRoot.transform);

        Vector3[] pillarPositions =
        {
            new Vector3(-4.5f, 1.75f, -4.5f),
            new Vector3( 4.5f, 1.75f, -4.5f),
            new Vector3(-4.5f, 1.75f,  4.5f),
            new Vector3( 4.5f, 1.75f,  4.5f),
        };

        for (int i = 0; i < pillarPositions.Length; i++)
        {
            GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pillar.name = $"Pillar_{i + 1}";
            pillar.transform.SetParent(pillars.transform);
            pillar.transform.position = pillarPositions[i];
            pillar.transform.localScale = new Vector3(0.65f, 3.5f, 0.65f);
            pillar.GetComponent<MeshRenderer>().sharedMaterial = pillarMaterial;
        }

        GameObject orbitRig = new GameObject("OrbitRig_ParentChildTransform");
        orbitRig.transform.SetParent(galleryRoot.transform);
        orbitRig.transform.position = new Vector3(0f, 3.15f, 0f);

        GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        orb.name = "GlowOrb_Child";
        orb.transform.SetParent(orbitRig.transform);
        orb.transform.localPosition = new Vector3(2.65f, 0f, 0f);
        orb.transform.localScale = Vector3.one * 0.55f;
        orb.GetComponent<MeshRenderer>().sharedMaterial = glowMaterial;

        Mid_OrbitingRelic orbit = orbitRig.AddComponent<Mid_OrbitingRelic>();
        orbit.floatingTarget = orb.transform;

        Light orbLight = orb.AddComponent<Light>();
        orbLight.type = LightType.Point;
        orbLight.color = new Color(0.18f, 0.75f, 1f);
        orbLight.intensity = 6f;
        orbLight.range = 9f;

        GameObject directionalObject = new GameObject("Moon_DirectionalLight");
        directionalObject.transform.SetParent(galleryRoot.transform);
        directionalObject.transform.rotation = Quaternion.Euler(42f, -28f, 0f);
        Light directional = directionalObject.AddComponent<Light>();
        directional.type = LightType.Directional;
        directional.color = new Color(0.46f, 0.58f, 0.80f);
        directional.intensity = 1.15f;

        GameObject fillLightObject = new GameObject("Altar_FillLight");
        fillLightObject.transform.SetParent(galleryRoot.transform);
        fillLightObject.transform.position = new Vector3(0f, 4f, 0f);
        Light fillLight = fillLightObject.AddComponent<Light>();
        fillLight.type = LightType.Point;
        fillLight.color = new Color(0.20f, 0.65f, 1f);
        fillLight.intensity = 4f;
        fillLight.range = 10f;

        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.AddComponent<Camera>();
        cameraObject.AddComponent<AudioListener>();
        cameraObject.transform.position = new Vector3(10f, 7f, -12f);
        cameraObject.transform.LookAt(new Vector3(0f, 1.4f, 0f));
        camera.clearFlags = CameraClearFlags.Skybox;
        camera.fieldOfView = 48f;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 100f;

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.08f, 0.10f, 0.18f);
        RenderSettings.ambientIntensity = 0.75f;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, ScenePath);
        AddSceneToBuildSettings(ScenePath);

        Selection.activeGameObject = altar;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Gallery scene created: {ScenePath}");
    }

    [MenuItem("CG/Mid/Build Pavilion Gallery Scene")]
    public static void BuildPavilionScene()
    {
        EnsureFolder("Assets", "Mid");
        EnsureFolder(MidRoot, "Materials");

        Material altarMaterial = CreateMaterial(
            MaterialRoot + "/Mat_Mid_Altar.mat",
            new Color(0.12f, 0.32f, 0.36f, 1f),
            0.35f,
            0.50f
        );

        Material glowMaterial = CreateEmissionMaterial(
            MaterialRoot + "/Mat_Mid_GlowOrb.mat",
            new Color(0.18f, 0.75f, 1.0f, 1f),
            4.5f
        );

        Scene scene = EditorSceneManager.OpenScene(
            PavilionSourcePath,
            OpenSceneMode.Single
        );

        GameObject previousAdditions = GameObject.Find("MidGallery_PavilionAdditions");

        if (previousAdditions != null)
        {
            Camera preservedCamera = previousAdditions.GetComponentInChildren<Camera>(true);

            if (preservedCamera != null)
            {
                preservedCamera.transform.SetParent(null, true);
            }

            Object.DestroyImmediate(previousAdditions);
        }

        GameObject galleryRoot = new GameObject("MidGallery_PavilionAdditions");

        GameObject altar = new GameObject("ProceduralAltar_LoopGenerated");
        altar.transform.SetParent(galleryRoot.transform);
        altar.transform.position = new Vector3(0f, 0.7f, 0f);
        altar.transform.localScale = Vector3.one * 0.38f;
        altar.AddComponent<Mid_ProceduralAltar>();
        altar.GetComponent<MeshRenderer>().sharedMaterial = altarMaterial;

        GameObject orbitRig = new GameObject("OrbitRig_ParentChildTransform");
        orbitRig.transform.SetParent(galleryRoot.transform);
        orbitRig.transform.position = new Vector3(0f, 1.35f, 0f);

        GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        orb.name = "GlowOrb_Child";
        orb.transform.SetParent(orbitRig.transform);
        orb.transform.localPosition = new Vector3(0.85f, 0f, 0f);
        orb.transform.localScale = Vector3.one * 0.18f;
        orb.GetComponent<MeshRenderer>().sharedMaterial = glowMaterial;

        Mid_OrbitingRelic orbit = orbitRig.AddComponent<Mid_OrbitingRelic>();
        orbit.floatingTarget = orb.transform;

        Light orbLight = orb.AddComponent<Light>();
        orbLight.type = LightType.Point;
        orbLight.color = new Color(0.18f, 0.75f, 1f);
        orbLight.intensity = 2.5f;
        orbLight.range = 4f;

        GameObject accentLightObject = new GameObject("Altar_AccentLight");
        accentLightObject.transform.SetParent(galleryRoot.transform);
        accentLightObject.transform.position = new Vector3(0f, 1.75f, 0f);

        Light accentLight = accentLightObject.AddComponent<Light>();
        accentLight.type = LightType.Point;
        accentLight.color = new Color(0.16f, 0.55f, 1f);
        accentLight.intensity = 2f;
        accentLight.range = 5f;

        GameObject sourceDiamond = GameObject.Find("Still_Diamond");
        if (sourceDiamond != null)
        {
            GameObject spinningDiamond = Object.Instantiate(sourceDiamond);
            spinningDiamond.name = "Gallery_RotatingDiamond";
            spinningDiamond.transform.SetParent(galleryRoot.transform);
            spinningDiamond.transform.position = new Vector3(-1.25f, 0.9f, 0f);
            spinningDiamond.transform.localScale = Vector3.one * 0.4f;
            spinningDiamond.AddComponent<Mid_Spin>();
        }

        GameObject sourcePyramid = GameObject.Find("Still_Pyramid");
        if (sourcePyramid != null)
        {
            GameObject bobbingPyramid = Object.Instantiate(sourcePyramid);
            bobbingPyramid.name = "Gallery_BobbingPyramid";
            bobbingPyramid.transform.SetParent(galleryRoot.transform);
            bobbingPyramid.transform.position = new Vector3(1.25f, 0.9f, 0f);
            bobbingPyramid.transform.localScale = Vector3.one * 0.4f;
            bobbingPyramid.AddComponent<Mid_Bob>();
        }

        ConfigureFirstPersonWalker(galleryRoot);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, PavilionScenePath);
        AddSceneToBuildSettings(PavilionScenePath);

        Selection.activeGameObject = altar;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Pavilion gallery scene created: {PavilionScenePath}");
    }

    private static void ConfigureFirstPersonWalker(GameObject galleryRoot)
    {
        SceneWalker existingWalker = Object.FindFirstObjectByType<SceneWalker>();

        if (existingWalker != null)
        {
            Camera existingCamera = Camera.main;

            if (existingCamera != null)
            {
                existingWalker.cameraTransform = existingCamera.transform;
            }

            return;
        }

        Camera sceneCamera = Camera.main;

        if (sceneCamera == null)
        {
            sceneCamera = Object.FindFirstObjectByType<Camera>();
        }

        if (sceneCamera == null)
        {
            Debug.LogWarning("Pavilion scene camera was not found.");
            return;
        }

        Transform cameraTransform = sceneCamera.transform;
        Vector3 cameraPosition = cameraTransform.position;
        Vector3 cameraEuler = cameraTransform.eulerAngles;

        GameObject player = new GameObject("Player_SceneWalker");
        player.transform.SetParent(galleryRoot.transform);
        player.transform.position = cameraPosition - Vector3.up * 1.6f;
        player.transform.rotation = Quaternion.Euler(0f, cameraEuler.y, 0f);

        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 1.8f;
        controller.radius = 0.30f;
        controller.center = new Vector3(0f, 0.9f, 0f);
        controller.stepOffset = 0.3f;

        cameraTransform.SetParent(player.transform, false);
        cameraTransform.localPosition = new Vector3(0f, 1.6f, 0f);
        cameraTransform.localRotation = Quaternion.Euler(cameraEuler.x, 0f, 0f);

        SceneWalker walker = player.AddComponent<SceneWalker>();
        walker.cameraTransform = cameraTransform;
        walker.moveSpeed = 2.5f;
        walker.runMultiplier = 2f;
        walker.lookSensitivity = 0.1f;

        GameObject floorCollider = new GameObject("WalkerFloorCollider");
        floorCollider.transform.SetParent(galleryRoot.transform);
        floorCollider.transform.position = new Vector3(0f, -0.12f, 0f);
        BoxCollider box = floorCollider.AddComponent<BoxCollider>();
        box.size = new Vector3(24f, 0.2f, 24f);
    }

    private static Material CreateMaterial(string path, Color color, float metallic, float smoothness)
    {
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

        if (material == null)
        {
            material = new Material(FindLitShader());
            AssetDatabase.CreateAsset(material, path);
        }

        SetColor(material, color);

        if (material.HasProperty("_Metallic"))
        {
            material.SetFloat("_Metallic", metallic);
        }

        if (material.HasProperty("_Smoothness"))
        {
            material.SetFloat("_Smoothness", smoothness);
        }

        EditorUtility.SetDirty(material);
        return material;
    }

    private static Material CreateEmissionMaterial(string path, Color color, float intensity)
    {
        Material material = CreateMaterial(path, color * 0.35f, 0.05f, 0.65f);
        Color emission = color * intensity;

        if (material.HasProperty("_EmissionColor"))
        {
            material.SetColor("_EmissionColor", emission);
            material.EnableKeyword("_EMISSION");
        }

        EditorUtility.SetDirty(material);
        return material;
    }

    private static Shader FindLitShader()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        return shader;
    }

    private static void SetColor(Material material, Color color)
    {
        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", color);
        }
    }

    private static void EnsureFolder(string parent, string child)
    {
        string path = parent + "/" + child;

        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(parent, child);
        }
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        if (EditorBuildSettings.scenes.Any(scene => scene.path == scenePath))
        {
            return;
        }

        EditorBuildSettings.scenes = EditorBuildSettings.scenes
            .Concat(new[] { new EditorBuildSettingsScene(scenePath, true) })
            .ToArray();
    }
}
