using System;  
using System.Diagnostics;  
using System.Text;  

public class AdvancedRomanConverter  
{  
    // AI-driven fractional part mapping  
    private static string MapFractionToRoman(double fraction)  
    {  
        double[] commonFractions = { 0.1, 0.125, 0.1667, 0.2, 0.25, 0.333, 0.5 };  
        string[] romanFractions = { "⅒", "⅛", "⅙", "⅕", "¼", "⅓", "½" };  

        double minDistance = double.MaxValue;  
        string closestRomanFraction = "";  
        for (int i = 0; i < commonFractions.Length; i++)  
        {  
            double distance = Math.Abs(fraction - commonFractions[i]);  
            if (distance < minDistance)  
            {  
                minDistance = distance;  
                closestRomanFraction = romanFractions[i];  
            }  
        }  

        return closestRomanFraction;  
    }  

    // Convert integer part to Roman numerals (handles up to 9999)  
    private static string IntegerToRoman(int number)  
    {  
        if (number < 1 || number > 9999)  
            throw new ArgumentOutOfRangeException("Number must be between 1 and 9999.");  

        StringBuilder result = new StringBuilder();  
        int[] digitsValues = { 10000, 9000, 5000, 4000, 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };  
        string[] romanDigits = { "ↂ", "Mↂ", "ↁ", "Mↁ", "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };  

        for (int i = 0; i < digitsValues.Length; i++)  
        {  
            while (number >= digitsValues[i])  
            {  
                result.Append(romanDigits[i]);  
                number -= digitsValues[i];  
            }  
        }  

        return result.ToString();  
    }  

    // Convert floating-point number to Roman numerals  
    public static string FloatToRoman(double number)  
    {  
        int integerPart = (int)number;  
        double fractionalPart = number - integerPart;  

        string romanInteger = IntegerToRoman(integerPart);  
        string romanFraction = fractionalPart > 0 ? MapFractionToRoman(fractionalPart) : "";  

        return romanInteger + romanFraction;  
    }  

    // Inverse process: Convert Roman numeral (with fractions) back to double  
    private static double RomanToFloat(string roman)  
    {  
        double result = 0;  

        // Convert integer part  
        int i = 0;  
        while (i < roman.Length)  
        {  
            if (i + 1 < roman.Length && RomanDigitValue(roman[i]) < RomanDigitValue(roman[i + 1]))  
            {  
                result += RomanDigitValue(roman[i + 1]) - RomanDigitValue(roman[i]);  
                i += 2;  
            }  
            else  
            {  
                result += RomanDigitValue(roman[i]);  
                i++;  
            }  
        }  

        // Convert fractional part  
        if (roman.Contains("½")) result += 0.5;  
        else if (roman.Contains("⅓")) result += 0.333;  
        else if (roman.Contains("¼")) result += 0.25;  
        else if (roman.Contains("⅕")) result += 0.2;  
        else if (roman.Contains("⅙")) result += 0.1667;  
        else if (roman.Contains("⅛")) result += 0.125;  
        else if (roman.Contains("⅒")) result += 0.1;  

        return result;  
    }  

    // Helper function to get the value of a Roman digit  
    private static int RomanDigitValue(char romanDigit)  
    {  
        switch (romanDigit)  
        {  
            case 'ↂ': return 10000;  
            case 'ↁ': return 5000;  
            case 'M': return 1000;  
            case 'D': return 500;  
            case 'C': return 100;  
            case 'L': return 50;  
            case 'X': return 10;  
            case 'V': return 5;  
            case 'I': return 1;  
            default: return 0;  
        }  
    }  

    // AI-driven accuracy calculation  
    private static double CalculateAccuracy(double original, double reconstructed)  
    {  
        double error = Math.Abs(original - reconstructed);  
        return (1 - (error / original)) * 100;  
    }  

    // Main method (demonstration)  
    public static void Main(string[] args)  
    {  
        double[] numbers = { 7300.5, 123.25, 456.333, 789.1, 1000.125 };  

        Stopwatch stopwatch = new Stopwatch();  
        foreach (double number in numbers)  
        {  
            try  
            {  
                stopwatch.Start();  
                string roman = FloatToRoman(number);  
                stopwatch.Stop();  
                double timeTaken = stopwatch.Elapsed.TotalMilliseconds;  

                double reconstructed = RomanToFloat(roman);  
                double accuracy = CalculateAccuracy(number, reconstructed);  

                Console.WriteLine($"Original: {number}, Roman: {roman}");  
                Console.WriteLine($"Reconstructed: {reconstructed}, Accuracy: {accuracy:F2}%");  
                Console.WriteLine($"Time Taken: {timeTaken:F4} ms");  
                Console.WriteLine();  
            }  
            catch (ArgumentOutOfRangeException ex)  
            {  
                Console.WriteLine($"Error processing {number}: {ex.Message}");  
            }  
            finally  
            {  
                stopwatch.Reset();  
            }  
        }  
    }  
}  
