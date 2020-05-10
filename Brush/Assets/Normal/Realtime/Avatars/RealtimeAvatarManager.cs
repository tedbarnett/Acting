using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Serialization;

namespace Normal.Realtime {
    [RequireComponent(typeof(Realtime))]
    public class RealtimeAvatarManager : MonoBehaviour {
#pragma warning disable 0649 // Disable variable is never assigned to warning.
        [FormerlySerializedAs("_avatarPrefab")]
        [SerializeField] private GameObject _localAvatarPrefab;
        [SerializeField] private RealtimeAvatar.LocalPlayer _localPlayer;
#pragma warning restore 0649

        public Dictionary<int, RealtimeAvatar> avatars     { get; private set; }
        public RealtimeAvatar                  localAvatar { get; private set; }

        private Realtime _realtime;

        void Awake() {
            _realtime = GetComponent<Realtime>();
            _realtime.didConnectToRoom += DidConnectToRoom;

            if (_localPlayer == null)
                _localPlayer = new RealtimeAvatar.LocalPlayer();

            avatars = new Dictionary<int, RealtimeAvatar>();
        }

        private void OnEnable() {
            // This is just here so we get the enable/disable checkbox in the inspector.
        }

        private void OnDisable() {
            // This is just here so we get the enable/disable checkbox in the inspector.
            // TODO: Should we destroy the avatar when this component is disabled?
        }

        void OnDestroy() {
            _realtime.didConnectToRoom -= DidConnectToRoom;
        }

        void DidConnectToRoom(Realtime room) {
            if (!gameObject.activeInHierarchy || !enabled)
                return;

            // Create avatar
            if (_localAvatarPrefab == null) {
                Debug.LogWarning("Realtime Avatars local avatar prefab is null. No avatar prefab will be instantiated for the local player.");
                return;
            }
            GameObject avatarGameObject = Realtime.Instantiate(_localAvatarPrefab.name, true, true, true, _realtime);
            if (avatarGameObject == null) {
                Debug.LogError("RealtimeAvatarManager: Failed to instantiate RealtimeAvatar prefab for the local player.");
                return;
            }
            localAvatar = avatarGameObject.GetComponent<RealtimeAvatar>();
            if (avatarGameObject == null) {
                Debug.LogError("RealtimeAvatarManager: Successfully instantiated avatar prefab, but could not find the RealtimeAvatar component.");
                return;
            }
            localAvatar.localPlayer = _localPlayer;
            localAvatar.deviceType = GetRealtimeAvatarDeviceTypeForLocalPlayer();
        }

        public static RealtimeAvatar.DeviceType GetRealtimeAvatarDeviceTypeForLocalPlayer() {
            switch (XRSettings.loadedDeviceName) {
                case "OpenVR":
                    return RealtimeAvatar.DeviceType.OpenVR;
                case "Oculus":
                    return RealtimeAvatar.DeviceType.Oculus;
                default:
                    return RealtimeAvatar.DeviceType.Unknown;
            }
        }

        public void _RegisterAvatar(int clientID, RealtimeAvatar avatar) {
            if (avatars.ContainsKey(clientID)) {
                Debug.LogError("RealtimeAvatar registered more than once for the same clientID (" + clientID + "). This is a bug!");
            }
            avatars[clientID] = avatar;
        }

        public void _UnregisterAvatar(RealtimeAvatar avatar) {
            List<KeyValuePair<int, RealtimeAvatar>> matchingAvatars = avatars.Where(keyValuePair => keyValuePair.Value == avatar).ToList();
            foreach (KeyValuePair<int, RealtimeAvatar> matchingAvatar in matchingAvatars) {
                avatars.Remove(matchingAvatar.Key);
            }
        }
    }
}
