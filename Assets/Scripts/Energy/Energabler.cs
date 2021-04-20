using UnityEngine;
public class Energabler : MonoBehaviour
{
    public int energy = 0;
    public int max_energy = 100;
    public bool AddEnergy()
    {
        if (IsFull())
        {
            return false;
        }
        energy++;
        return true;
    }

    public bool RemEnergy()
    {
        if (IsEmpty())
        {
            return false;
        }
        energy--;
        return true;
    }

    public bool IsFull()
    {
        return energy == max_energy;
    }

    public bool IsEmpty()
    {
        return energy == 0;
    }
}
