using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour {
    private NetworkVariable<PlayerNetworkData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);
    private GameObject TankTop_Pivot;
    private GameObject Tank_Base;

    // Start is called before the first frame update
    void Start()
    {
        TankTop_Pivot = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        Tank_Base = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner) {
            _netState.Value = new PlayerNetworkData() {
                Position = Tank_Base.transform.position,
                Rotation = Tank_Base.transform.rotation.eulerAngles,
                GunRotation = TankTop_Pivot.transform.rotation.eulerAngles
            };
        }
        else {
            Tank_Base.transform.position = _netState.Value.Position;
            Tank_Base.transform.rotation = Quaternion.Euler(_netState.Value.Rotation);
            TankTop_Pivot.transform.rotation = Quaternion.Euler(_netState.Value.GunRotation);
        }
    }

    struct PlayerNetworkData : INetworkSerializable{
        private float _x, _y;
        private float _zRot;
        private float _zGRot;

        internal Vector3 Position {
            get => new Vector3(_x,_y,0);
            set {
                _x = value.x;
                _y = value.y;
            }
        }

        internal Vector3 Rotation {
            get => new Vector3(0,0,_zRot);
            set => _zRot = value.z;
        }

        internal Vector3 GunRotation {
            get => new Vector3(0,0,_zGRot);
            set => _zGRot = value.z;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);

            serializer.SerializeValue(ref _zRot);
            serializer.SerializeValue(ref _zGRot);
        } 
    }
}
