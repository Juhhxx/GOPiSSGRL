using UnityEngine;
using System.Collections;

public class SlushieMaker : MonoBehaviour
{
    [SerializeField] private GameObject _slushieParticles;
    [SerializeField] private Flavours _slushieFlavour;
    [SerializeField] private float _slushieTime;
    private Transform _correctTrans;
    private YieldInstruction _waitSeconds;

    private void Start()
    {
        _correctTrans = _slushieParticles.transform;
        _waitSeconds = new WaitForSeconds(_slushieTime);
    }
    public void MakeSlushie()
    {
        CheckForCup();
    }
    private void CheckForCup()
    {
        RaycastHit hit;

        Debug.DrawRay(_correctTrans.position,_correctTrans.forward*0.5f,Color.blue);
        if (Physics.Raycast( _correctTrans.position, _correctTrans.forward, out hit, 0.5f))
        {
            SlushieCup cup = hit.collider.gameObject.GetComponent<SlushieCup>();
            if(cup != null)
                StartCoroutine(PourSlushie(cup));
        }
    }
    private IEnumerator PourSlushie(SlushieCup cup)
    {
        _slushieParticles.SetActive(true);
        cup.AddFlavour(_slushieFlavour);

        yield return _waitSeconds;
        _slushieParticles.SetActive(false);
    }

}
