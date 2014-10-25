using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tank : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods
    
    public void Init(IInputProvider iInputProvider, Action<Chunk> onEnterToChunk)
    {
        _iInputProvider = iInputProvider;
        _onEnterToChunk = onEnterToChunk;
    }

    public void Reset()
    {
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region events
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Chunk chunk = col.transform.parent.GetComponent<Chunk>();
        if(chunk != null)
        {
            Utils.InvokeAction(_onEnterToChunk, chunk);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoDevelop

    private void Update()
    {
        Vector2 dir = Utils.DegreesToVector2(-transform.localEulerAngles.z);
        rigidbody2D.velocity = dir * Settings.Instance.TankMovingSpeed * _iInputProvider.InputY;

        rigidbody2D.angularVelocity = -Settings.Instance.TankRotationSpeed * _iInputProvider.InputX;
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private IInputProvider _iInputProvider;
    private Action<Chunk> _onEnterToChunk;

    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
