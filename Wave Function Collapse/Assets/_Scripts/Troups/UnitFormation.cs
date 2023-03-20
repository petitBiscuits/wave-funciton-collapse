using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used to give a pattern for the formation of the units.
/// </summary>
public class UnitFormation
{

    public static Vector3[] GetFormationPositions(int numberOfUnits, float formationRadius, float formationAngle)
    {
        Vector3[] formationPositions = new Vector3[numberOfUnits];
        float angle = 0f;
        for (int i = 0; i < numberOfUnits; i++)
        {
            formationPositions[i] = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * formationRadius;
            angle += formationAngle;
        }
        return formationPositions;
    }
}
