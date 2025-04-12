using UnityEngine;
public class LevelManager : MonoBehaviour
{


    private readonly float LEVEL_DURATION = 60f;
    private bool _mInitialised, _mCreatedPortal;
    private float _mStartTime;

	private GameObject _mLevelGenerator;
    
    public void Initialize()
    {
        _mStartTime = Time.time;
        _mInitialised = true;
		_mLevelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator");
    }

    private void Update()
    {
        if (!_mInitialised) return;
        if (_mCreatedPortal || !(Time.time - _mStartTime > LEVEL_DURATION)) return;
        
        _mCreatedPortal = true;
        var portal = GameObject.FindGameObjectWithTag("Portal");
        portal.transform.localScale = new Vector3(100, 100, 100);
        
        _mLevelGenerator.SetActive(false);
        var portalRelocator = _mLevelGenerator.AddComponent(typeof(Relocator)) as Relocator;
        portalRelocator.ToRelocate = new []{portal};
        _mLevelGenerator.SetActive(true);
    }
}