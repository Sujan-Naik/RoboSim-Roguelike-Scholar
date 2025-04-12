using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

	public float kMaxHealth = 100f;

	private float _mHealth;

	public GameObject kImageObject;
	private Image _mImage;

    private void Start()
    {
	    _mHealth = kMaxHealth;
	    _mImage = kImageObject.GetComponent<Image>();
	    _mImage.type = Image.Type.Filled;
	    _mImage.fillMethod = _mImage.flexibleHeight < _mImage.flexibleWidth ? Image.FillMethod.Vertical : Image.FillMethod.Horizontal;
	    _mImage.fillAmount = 0.99f;
    }

    public void Damage(float amount)
    {

	    var newHealth = _mHealth - amount;
	    if (newHealth <= 0)
	    {
	     _mHealth = 0;
	     _mImage.fillAmount = 0; 
	    }
	    else
	    {
	     _mImage.fillAmount = newHealth / kMaxHealth;
	     _mHealth = newHealth;
	    }
    }
    
    public void Heal(float amount)
    {
	    _mHealth += amount;
    }

    void Update()
    {
	    if (_mHealth == 0)
	    {
		    if (CompareTag("Player"))
		    {
			    SceneManager.LoadScene(GameplayManager.TUTORIAL);
		    }
		    else
		    {
			    Destroy(gameObject);
		    }
	    }
    }
}
