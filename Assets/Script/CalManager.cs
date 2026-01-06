using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalManager : MonoBehaviour
{
    public TextMeshProUGUI resText;
    public TextMeshProUGUI input;
    public TextMeshProUGUI oldInput;
    bool error;
    string currInput = "";

    bool CheckOp(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/';
    }
    public void AddOp(string op)
    {
        if (string.IsNullOrEmpty(currInput)) return;

        char last = currInput[currInput.Length-1];
        if (CheckOp(last)) return;

        currInput  += op;
        resText.text = currInput;
        //oldInput.text = currInput;
    }

    public void AddNum(string num)
    {
        if (num == "." && currInput.Length > 0)
        {
            int i = currInput.Length - 1;
            while (i >= 0 && !CheckOp(currInput[i]))
            {
                if (currInput[i] == '.')
                    return;
                i--;
            }
        }
        currInput += num;
        resText.text = currInput;
        //oldInput.text = currInput;
    }

    public void ResetCal()
    {
        currInput = "";
        resText.text = "";
        input.text = "";
        oldInput.text = "";
    }

    public void Clear()
    {
        if (string.IsNullOrEmpty(currInput)) return;

        currInput = currInput.Substring(0, currInput.Length-1);
        resText.text = currInput;
        oldInput.text = currInput;
    }


    List<string> CheckExp(string input)
    {
        List<string> exp = new List<string>();
        string temp = "";

        foreach(char c in input)
        {
            if(char.IsDigit(c) || c == '.')
                temp += c;
            else if(CheckOp(c))
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    exp.Add(temp);
                    exp.Add(c.ToString());
                    temp = "";
                }
            }
        }

        if(!string.IsNullOrEmpty(temp))
            exp.Add(temp);
        
        return exp;
    }

    float Calculate(string input)
    {
        List<string> exp = CheckExp(input);
        

        for(int i = 0; i < exp.Count; i++)
        {
            if(exp[i] == "/" || exp[i] == "*")
            {
                float l = float.Parse(exp[i-1]); //num before '/' op
                float r = float.Parse(exp[i+1]); //num after '*' op

                if (exp[i] == "/" && r == 0) //handle divide by 0
                {
                    resText.text = "Error";
                    currInput = "";
                    error = true;
                    return 0f;
                }
                float value = exp[i] == "*" ? l*r : l/r;

                exp[i-1] = value.ToString();
                exp.RemoveAt(i); //op
                exp.RemoveAt(i); //num at r
                i--;
            }
        }

        float result = float.Parse(exp[0]);
        for(int i = 1; i < exp.Count; i +=2 )
        {
            float nextVal = float.Parse(exp[i+1]);
            if(exp[i] == "+")
                result += nextVal;
            else
                result -= nextVal;
        }

        return result;
    }

    public void Result()
    {
        if (string.IsNullOrEmpty(currInput)) return;

        error = false;
        oldInput.text = currInput;
        float result = Calculate(currInput);
        
        if(error) return;

        resText.text = result.ToString();
    }
}
