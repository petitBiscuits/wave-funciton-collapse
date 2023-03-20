using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Renderer[] _material;

    // Start is called before the first frame update
    void Start()
    {
        UnitSelections.Instance.unitList.Add(this.gameObject);
        
        _material = GetComponentsInChildren<Renderer>();

        StartCoroutine("Levitating");
    }

    void Update()
    {
        
    }

    IEnumerator Levitating()
    {
        float x = 0;
        while (true)
        {
            float y = Mathf.Sin(x) / 10;

            this.transform.position += new Vector3(0, y, 0);

            yield return null;

            x += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        UnitSelections.Instance.unitList.Remove(this.gameObject);
    }

    public void ShowHighlight()
    {
        foreach (var rend in _material)
        {
            rend.material.SetInt("_isHighLight", 1);
        }
    }

    public void HideHighlight()
    {
        foreach (var rend in _material)
        {
            rend.material.SetInt("_isHighLight", 0);
        }
    }
}
