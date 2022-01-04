using System;
using System.Collections;
using System.Collections.Generic;

public static class Utils
{
    private static Random random = new Random();

    /// <summary>
    /// Shuffles an Array Using the Fisher-Yates Shuffle Algorithm.
    /// </summary>
    /// <param name="_arrayToShuffle">Array To Shuffle.</param>
    /// <typeparam name="T">Array Element Type.</typeparam>
    public static T[] SuffleArray<T>(T[] _arrayToShuffle)
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

        return _arrayToShuffle;
    }

    /// <summary>
    /// Shuffles a List Using the Fisher-Yates Shuffle Algorithm.
    /// </summary>
    /// <param name="_ListToShuffle">List To Shuffle.</param>
    /// <typeparam name="T">List Element Type.</typeparam>
    /// <returns></returns>
    public static List<T> ShuffleList<T>(List<T> _listToShuffle)
    {
        int selection = 0;

        int index = _listToShuffle.Count;
        while (index > 0)
        {
            selection = random.Next(index--);
            T temp = _listToShuffle[index];
            _listToShuffle[index] = _listToShuffle[selection];
            _listToShuffle[selection] = temp;
        }

        return _listToShuffle;
    }
}
