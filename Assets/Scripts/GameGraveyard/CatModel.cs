using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CatModel : MonoBehaviour
{
    [Header("GRAPHICS TWEAKING")] 
    public float scaleMultiplier = 1.5f;
    
    [Header("MESH RENDERER")]
    public SkinnedMeshRenderer catMeshRenderer;
    public SkinnedMeshRenderer catEyesMeshRenderer;

    [Header("BONES")] 
    public GameObject boneHead;
    
    
    private Material _catSkinMateriel;
    private Texture _catSkinTexture;
    
    private Material _catEyesMaterial;
    private Texture _catEyesTexture;
    
    private GameObject _headAddon;
    private GameObject _rightHandAddon;
    private GameObject _leftHandAddon;
    
    public void UpdateGraphics(int catType)
    {
        transform.localScale = Vector3.one;
        transform.localScale *= scaleMultiplier;
        
        // skin material update
        _catSkinTexture = Registry.entitiesConfig.cats[catType].catSkinTexture;
        _catSkinMateriel = catMeshRenderer.material;
        _catSkinMateriel.SetTexture("_MainTex", _catSkinTexture);
        
        // eyes material update
        _catEyesTexture = Registry.entitiesConfig.cats[catType].catEyesTexture;
        _catEyesMaterial = catEyesMeshRenderer.material;
        _catEyesMaterial.SetTexture("_MainTex", _catEyesTexture);

        // graphics instantiate addons
        if (_headAddon) Destroy(_headAddon.gameObject);
        _headAddon = InstantiateAddon(Registry.entitiesConfig.cats[catType].headAddon, boneHead.transform);
        if (_headAddon) _headAddon.SetActive(true);
    }
    
    /// <summary>
    /// Create a new addon and snap it to the desired bone
    /// </summary>
    /// <returns>The newly instantiated addon</returns>
    private GameObject InstantiateAddon(GameObject addonToInstantiate, Transform bone)
    {
        // exit if the addon doesn't exist
        if (!addonToInstantiate) return null;
        
        var newAddon = Instantiate(addonToInstantiate, bone, true);
        newAddon.transform.localPosition = Vector3.zero;
        newAddon.transform.localRotation = Quaternion.identity;
        newAddon.transform.localScale *= scaleMultiplier;
        newAddon.SetActive(false);
        
        return newAddon;
    }
}