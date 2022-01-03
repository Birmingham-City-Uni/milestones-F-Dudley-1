using System;
using System.Collections;
using System.Collections.Generic;

public static class Utils
{
    /// <summary>
    /// Shuffles an Array Using the Fisher-Yates Shuffle Algorithm.
    /// </summary>
    /// <param name="_arrayToShuffle">Array To Shuffle.</param>
    /// <typeparam name="T">Array Element Type.</typeparam>
    public static void SuffleArray<T>(ref T[] _arrayToShuffle)
    {
        Random random = new Random();
        int selection = 0;

        for (int i = 0; i < _arrayToShuffle.Length; i++)
        {
            selection = random.Next(i, _arrayToShuffle.Length - 1);
            T temp = _arrayToShuffle[i];
            _arrayToShuffle[i] = _arrayToShuffle[selection];
            _arrayToShuffle[selection] = temp;
        }
    }

    /// <summary>
    /// Shuffles a List Using the Fisher-Yates Shuffle Algorithm.
    /// </summary>
    /// <param name="_ListToShuffle">List To Shuffle.</param>
    /// <typeparam name="T">List Element Type.</typeparam>
    /// <returns></returns>
    public static void ShuffleList<T>(ref List<T> _listToShuffle)
    {
        Random random = new Random();
        int selection = 0;

        for (int i = 0; i < _listToShuffle.Count; i++)
        {
            selection = random.Next(i, _listToShuffle.Count - 1);
            T temp = _listToShuffle[i];
            _listToShuffle[i] = _listToShuffle[selection];
            _listToShuffle[selection] = temp;
        }
    }
}
