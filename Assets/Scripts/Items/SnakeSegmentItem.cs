using UnityEngine;

namespace Snake3
{
    /**
     * Represents a snake segment item in game.
     *
     * Serves as a base class for SnakeHeadItem.
     */
    public class SnakeSegmentItem : MonoBehaviour
    {
        // ----------------------------
        // Fields
        // ----------------------------

        [SerializeField] private int _segmentNumber;
        [SerializeField] private SnakeSegmentItem _child;

        public SnakeSegmentItem Child => _child;  // For private setter.
        public Vector3Int Position { get; private set; }

        private bool _hasChild;

        // ----------------------------
        // Event Functions
        // ----------------------------

        protected virtual void Awake()
        {
            _hasChild = _child != null;
        }

        private void Start()
        {
            var startingPosition = Vector3Int.RoundToInt(transform.position);
            Position = startingPosition;
            transform.position = startingPosition;
        }

        // ----------------------------
        // Methods
        // ----------------------------

        /**
         * Move the segment to a given position and make other segments recursively follow.
         *
         * If createChild is true, spawns a new segment at the end.
         */
        protected void MoveSnakeChain(Vector3Int newPosition, bool createChild)
        {
            var oldPosition = Position;
            Position = newPosition;
            transform.position = newPosition;
            if (_hasChild)
            {
                _child.MoveSnakeChain(oldPosition, createChild);
            }
            else if (createChild)
            {
                CreateChild();
                _child.MoveSnakeChain(oldPosition, false);
            }
        }

        /**
         * Instantiate new SnakeSegment GameObject and set it as a child of this segment.
         *
         * The new segment is instantiated under the same parent as this one has.
         */
        private void CreateChild()
        {
            Debug.Assert(!_hasChild);

            _child = Instantiate(gameObject, transform.parent).GetComponent<SnakeSegmentItem>();
            _child.SetSegmentNumber(_segmentNumber + 1);
            _hasChild = true;
        }

        /**
         * Set segment number and rename the GameObject accordingly.
         */
        private void SetSegmentNumber(int num)
        {
            _segmentNumber = num;
            gameObject.name = "Segment " + num;
        }
    }
}
