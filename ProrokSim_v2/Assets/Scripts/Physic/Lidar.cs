using System.Collections.Generic;
using UnityEngine;

namespace Physic
{
    public class Lidar : MonoBehaviour
    {

        public int rayRange; /*    lidar's range   */
        public int horizontalRange; /*    half of lidar's horizontal angular range    */
        public int verticalRange; /*    half of lidar's vertical angular range    */
        public float horizontalStep; /*    angular step between to horizontal ray    */
        public float verticalStep; /*    angular step between to vertical ray    */
        public float horizontalOffset;    /*    horizontal offset the lidar (in degrees)     */
        public float verticalOffset;    /*    vertical offset the lidar (in degrees)     */

        public GameObject lidar;

        private static IEnumerable<LidarPoint> _measures;

        public static IEnumerable<LidarPoint> GetMeasures()
        {
            /*    Return Lidar's datas    */
            return _measures;
        }

        private void Start()
        {
            _measures = Scan();
        }

        private void Update()
        {
            _measures = Scan();
  
        }

        private IEnumerable<LidarPoint> Scan()
        {
            /*    Return all the point measured by the Lidar. Each point contain the horizontal angle,
         *    the vertical angle and the distance between the Lidar and the hit object.
         */
            var measures = new List<LidarPoint>();
            var position = lidar.transform.position;
            var lidarDirection = Quaternion.Euler(verticalOffset, horizontalOffset, 0) * lidar.transform.forward;

            for (float hAngle = -horizontalRange; hAngle <= horizontalRange; hAngle += horizontalStep)
            {
                for (float vAngle = -verticalRange; vAngle <= verticalRange; vAngle += verticalStep)
                {
                    var direction = Quaternion.Euler(vAngle, hAngle, 0) * lidarDirection;
                    if (Physics.Raycast(position, direction, out var hit, rayRange))
                    {
                        Debug.DrawRay(position, direction * hit.distance, Color.red);
                    }
                    else
                    {
                        Debug.DrawRay(position, direction * rayRange, Color.green);
                    }
                    measures.Add(new LidarPoint(hAngle, vAngle, hit.distance));
                }
            }
            return measures;
        }
    }
}
