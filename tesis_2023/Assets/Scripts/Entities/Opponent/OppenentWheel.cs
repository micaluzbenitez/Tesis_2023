using UnityEngine;

namespace Entities.Opponent
{
    public class OppenentWheel : MonoBehaviour
    {
        [Header("Car parent")]
        [SerializeField] private Transform car;

        [Header("Wheels")]
        [SerializeField] private Transform leftBackWheel;
        [SerializeField] private Transform leftFrontWheel;
        [SerializeField] private Transform rightBackWheel;
        [SerializeField] private Transform rightFrontWheel;

        [Header("Wheels limits")]
        [SerializeField] private float minLimit = -50f;
        [SerializeField] private float maxLimit = 50f;

        [Header("Wheels Z rotation")]
        [SerializeField] private float leftZRotation = 90f;
        [SerializeField] private float rightZRotation = -90f;

        private OpponentAI opponentAI;
        private float actualCarYRotation;
        private float carYRotation;

        private float yLeftRotation;
        private float yRightRotation;

        private void Awake()
        {
            opponentAI = GetComponent<OpponentAI>();

            actualCarYRotation = car.rotation.eulerAngles.y;   
            carYRotation = car.rotation.eulerAngles.y;   
        }

        private void Update()
        {
            // X rotation
            if (opponentAI.IsAlive())
            {
                float velocity = opponentAI.GetVelocity();
                float wheelsXRotation = velocity * Time.deltaTime * 1000f; // Adjust the 1000f factor according to your scale

                // Y rotation
                bool rotating = false;
                bool leftRotation = false;

                if (Mathf.Abs(actualCarYRotation - car.rotation.eulerAngles.y) > 0.1f) // If the car is turning
                {
                    if ((carYRotation - car.rotation.eulerAngles.y) > 0) leftRotation = true;
                    rotating = true;
                    actualCarYRotation = car.rotation.eulerAngles.y;
                }

                if (!rotating) carYRotation = car.rotation.eulerAngles.y;

                yLeftRotation = Mathf.Clamp(Mathf.Abs(car.rotation.eulerAngles.y - carYRotation), minLimit, maxLimit);
                yRightRotation = Mathf.Clamp(Mathf.Abs(car.rotation.eulerAngles.y - carYRotation), minLimit, maxLimit);

                if (leftRotation)
                {
                    yLeftRotation = -yLeftRotation;
                    yRightRotation = -yRightRotation;
                }

                // Apply front wheels rotation (X and Y rotation)
                Quaternion endLeftRotation = Quaternion.Euler(wheelsXRotation, yLeftRotation, leftZRotation);
                Quaternion endRightRotation = Quaternion.Euler(wheelsXRotation, yRightRotation, rightZRotation);

                leftFrontWheel.localRotation = Quaternion.Lerp(leftFrontWheel.localRotation, endLeftRotation, Time.deltaTime);
                rightFrontWheel.localRotation = Quaternion.Lerp(rightFrontWheel.localRotation, endRightRotation, Time.deltaTime);
                
                // Apply back wheels rotation (X rotation)
                leftBackWheel.localRotation = Quaternion.Euler(wheelsXRotation, 0, leftZRotation);
                rightBackWheel.localRotation = Quaternion.Euler(wheelsXRotation, 0, rightZRotation);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}