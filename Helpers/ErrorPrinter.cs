// M. Cihan Ozer - March 2017

using UnityEngine;

public static class ErrorPrinter
{
    public static void PrintError(string className, string functionName, string errorDetails = "", string errorBy = "")
    { 
        Debug.LogFormat(
                            "<color=#ff0000ff><b>ERROR</b></color> IN CLASS <color=#ffa500ff>{0}</color>" +
                            " IN METHOD <color=#d69848>{1}</color>" +
                            " ERROR DETAILS: <color=#0000ffff>{2}</color>" +
                            " ERROR BY: {3}",
                            className, functionName, errorDetails, errorBy
                        );
    }
}
