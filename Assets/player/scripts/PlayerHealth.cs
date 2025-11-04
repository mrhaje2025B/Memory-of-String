using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int max_hair = 80;
    public int hair;
    public int max_temp_hair = 80;
    public int temp_hair;
    public int temp_hair_decrease_rate = 8;
    private Coroutine THD;
    public int growth_per_sec = 8;
    public int temp_growth_per_sec = 16;


    IEnumerator Hair_growth()
    {
        while (true)
        {
            int regen = (temp_hair > 0) ? temp_growth_per_sec : growth_per_sec;
            if (hair < max_hair) { hair += regen; }
            if (hair > max_hair) { hair = max_hair; }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Temp_hair_decrease()
    {
        yield return new WaitForSeconds(1);
        while (temp_hair > 0)
        {
            
            temp_hair -= temp_hair_decrease_rate;
            if (temp_hair < 0) { temp_hair = 0; }

            yield return new WaitForSeconds(0.5f);
        }
        THD = null;
    }

    public int Hair_cut(int damage)
    {
        if (temp_hair > 0)
        {
            temp_hair -= damage;
            if (temp_hair < 0)
            {
                hair += temp_hair;
                temp_hair = 0;
            }
        }
        else { hair -= damage; }


        return damage;
    }

    public void Hair_steal(int amount)
    {
        if (temp_hair < max_temp_hair)
        { 
            temp_hair += amount;
            if (temp_hair > max_temp_hair) { temp_hair = max_temp_hair; }
        }
        if (THD != null) { StopCoroutine(THD); }
        THD = StartCoroutine(Temp_hair_decrease());

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hair = max_hair;
        //StartCoroutine(Hair_growth()); //전투방식 고민중, 자동회복은 일단 보류
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
