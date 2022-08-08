using UnityEngine;
using System;
using System.Collections.Generic;

public class OneEuroFilter
{
    float freq;
    float mincutoff;
    float beta;
    float dcutoff;
    LowPassFilter x;
    LowPassFilter dx;
    float lasttime;

    // currValue contains the latest value which have been succesfully filtered
    // prevValue contains the previous filtered value
    public float currValue { get; protected set; }
    public float prevValue { get; protected set; }

    float alpha(float _cutoff)
    {
        float te = 1.0f / freq;
        float tau = 1.0f / (2.0f * Mathf.PI * _cutoff);
        return 1.0f / (1.0f + tau / te);
    }

    void setFrequency(float _f)
    {
        if (_f <= 0.0f)
        {
            Debug.LogError("freq should be > 0");
            return;
        }
        freq = _f;
    }

    void setMinCutoff(float _mc)
    {
        if (_mc <= 0.0f)
        {
            Debug.LogError("mincutoff should be > 0");
            return;
        }
        mincutoff = _mc;
    }

    void setBeta(float _b)
    {
        beta = _b;
    }

    void setDerivateCutoff(float _dc)
    {
        if (_dc <= 0.0f)
        {
            Debug.LogError("dcutoff should be > 0");
            return;
        }
        dcutoff = _dc;
    }

    public OneEuroFilter(float _freq, float _mincutoff = 1.0f, float _beta = 0.0f, float _dcutoff = 1.0f)
    {
        setFrequency(_freq);
        setMinCutoff(_mincutoff);
        setBeta(_beta);
        setDerivateCutoff(_dcutoff);
        x = new LowPassFilter(alpha(mincutoff));
        dx = new LowPassFilter(alpha(dcutoff));
        lasttime = -1.0f;

        currValue = 0.0f;
        prevValue = currValue;
    }

    public void UpdateParams(float _freq, float _mincutoff = 1.0f, float _beta = 0.0f, float _dcutoff = 1.0f)
    {
        setFrequency(_freq);
        setMinCutoff(_mincutoff);
        setBeta(_beta);
        setDerivateCutoff(_dcutoff);
        x.setAlpha(alpha(mincutoff));
        dx.setAlpha(alpha(dcutoff));
    }

    public float Filter(float value, float timestamp = -1.0f)
    {
        prevValue = currValue;

        // update the sampling frequency based on timestamps
        if (lasttime != -1.0f && timestamp != -1.0f)
            freq = 1.0f / (timestamp - lasttime);
        lasttime = timestamp;
        // estimate the current variation per second 
        float dvalue = x.hasLastRawValue() ? (value - x.lastRawValue()) * freq : 0.0f; // FIXME: 0.0 or value? 
        float edvalue = dx.filterWithAlpha(dvalue, alpha(dcutoff));
        // use it to update the cutoff frequency
        float cutoff = mincutoff + beta * Mathf.Abs(edvalue);
        // filter the given value
        currValue = x.filterWithAlpha(value, alpha(cutoff));

        return currValue;
    }
}

