using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class is used to give a pattern for the formation of the units.
/// </summary>
public class UnitFormation
{

    /// <summary>
    /// Dinguerie bizare mais drôle
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static List<Vector3> GetMultipleCicle(GameObject parent)
    {
        int numberOfUnits = UnitSelections.Instance.unitsSelected.Count;

        List<Vector3> formationPositions = new List<Vector3>();
        
        float wUnit = 0.3f;
        float dCircle = 0.4f;

        int numberOfCircle = 0;
        int numberOfUnitPlaced = 0;

        List<Circle> circles = new List<Circle>();

        do
        {
            Circle temp = new Circle();
            temp.ID = numberOfCircle;
            temp.Perimeter = Mathf.PI * dCircle;
            temp.NumberOfUnit = (int)(temp.Perimeter / wUnit);
            numberOfUnitPlaced += temp.NumberOfUnit;
            numberOfCircle++;
            dCircle += 0.5f;
            circles.Add(temp);
        } while (numberOfUnitPlaced < numberOfUnits);

        foreach (Circle c in circles)
        {
            List<Vector3> listPoints = MakeCircle(c.NumberOfUnit, c.Perimeter, parent.transform.position, (c.ID % 2 == 0) ? true:false);
            foreach (Vector3 v in listPoints)
            {
                formationPositions.Add(v);
            }
        }

        return formationPositions;
    }

    private static List<Vector3> MakeCircle(int n, float radius, Vector3 center, bool offset)
    {
        List<Vector3> formationPositions = new List<Vector3>();

        float angleDEG = (360f / n);
        float formationAngle = Mathf.Deg2Rad * angleDEG;

        for (int i = 0; i < n; i++)
        {
            float angle = formationAngle * i;
            angle += (offset) ? Mathf.Deg2Rad * (angleDEG / 2) : 0;
            Vector3 formationPosition = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;

            formationPositions.Add(center + formationPosition);
        }

        return formationPositions;
    }

    public static List<Vector3> GetCircleFormation(GameObject parent)
    {
        int numberOfUnits = UnitSelections.Instance.unitsSelected.Count;

        List<Vector3> formationPositions = new List<Vector3>();

        float formationAngle = Mathf.Deg2Rad * (360f / numberOfUnits);

        float formationRadius = Mathf.Sqrt(numberOfUnits) * 2f;

        for (int i = 0; i < numberOfUnits; i++)
        {
            float angle =  formationAngle * i;
            Vector3 formationPosition = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * formationRadius;

            formationPositions.Add(parent.transform.position + formationPosition);
        }

        return formationPositions;
    }

    public static List<Vector3> GetRowFormation(GameObject parent)
    {
        int numberOfUnit = UnitSelections.Instance.unitsSelected.Count;

        List<Vector3> formationPositions = new List<Vector3>();

        float offset = 0;

        int x = 0;
        int z = -1;

        if (numberOfUnit % 2 == 0)
        {
            offset += (-1 * numberOfUnit / 2) + 0.5f;
        }
        else
        {
            offset += (-1 * numberOfUnit / 2);
        }

        for (int i = 0; i < numberOfUnit; i++)
        {
            formationPositions.Add(parent.transform.position + new Vector3(x + offset, 0, z));
            x += 1;
        }

        return formationPositions;
    }
}

public struct Circle
{
    public Circle(int id, float perimeter, int numberOfUnit)
    {
        ID = id;
        Perimeter = perimeter;
        NumberOfUnit = numberOfUnit;
    }
    
    public int ID { get; set; }
    public float Perimeter { get; set; }
    public int NumberOfUnit { get; set; }
}