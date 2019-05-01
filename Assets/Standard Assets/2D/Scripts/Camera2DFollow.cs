using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        // <<< BEGIN MODIF
        // Original
        //public Transform target;
        // Option 1
        private Vector3 target;
        public GameObject[] players;
        private float initialSize, initialPlayerDistance;
        public Camera mainCamera;
        public Camera[] playerCameras;
        public float maxAllowedDistance;
        // Option 2
        // public Transform target;
        // END MODIF >>>
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            // <<< BEGIN MODIF
            // Option 1
            initialSize = mainCamera.orthographicSize;
            initialPlayerDistance = GetPlayersMaxDistance();
            SetTargetPrivatly(); // Adition to all script: every 'target.position' becomes 'target'
            // END MODIF >>>

            m_LastTargetPosition = target;
            m_OffsetZ = (transform.position - target).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            // <<< BEGIN MODIF
            // Option 1
            SetTargetPrivatly();
            // END MODIF >>>

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = target;
        }

        private void SetTargetPrivatly()
        {
            target = Vector3.zero;

            foreach (GameObject player in players)
            {
                target += player.transform.position;
            }

            target /= players.Length;

            SetActiveCameras();

            if (mainCamera.enabled && GetPlayersMaxDistance() > initialPlayerDistance)
                SetCameraSize(); // Only should apply for main camera
        }

        private void SetCameraSize()
        {
            mainCamera.orthographicSize = GetPlayersMaxDistance() / initialPlayerDistance * initialSize;
        }

        private float GetPlayersMaxDistance()
        {
            if (players.Length == 1)
                return 0;

            return (players[0].transform.position - players[1].transform.position).magnitude;
        }

        private void SetActiveCameras()
        {
            if (GetPlayersMaxDistance() >= maxAllowedDistance)
            {
                mainCamera.enabled = false;
                foreach (Camera camera in playerCameras)
                {
                    camera.enabled = true;
                }
            }
            else
            {
                mainCamera.enabled = true;
                foreach (Camera camera in playerCameras)
                {
                    camera.enabled = false;
                }
            }
        }
    }
}
