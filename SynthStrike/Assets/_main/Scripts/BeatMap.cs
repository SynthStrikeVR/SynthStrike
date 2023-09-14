using System;
using System.Collections.Generic;

[Serializable]
public class BeatMap
{
    public List<MapNote> track1;
    public List<MapNote> track2;
    public List<MapNote> track3;
    public List<MapNote> track4;
    public List<MapNote> track5;
    public List<MapNote> track6;
    public List<MapNote> track7;
    public List<MapNote> track8;
    public List<MapNote> track9;

    public List<MapNote> GetTrack(int trackNo)
    {
        return trackNo switch
        {
            0 => track1,
            1 => track2,
            2 => track3,
            3 => track4,
            4 => track5,
            5 => track6,
            6 => track7,
            7 => track8,
            8 => track9,
            _ => null
        };
    }
}