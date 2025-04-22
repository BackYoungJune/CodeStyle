/*********************************************************					
* NeutralNPC_Bird.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dreamteck.Splines;				
using UnityEngine;					
								
namespace Dev_Unit				
{					
	public class NeutralNPC_Bird : MonoBehaviour					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					


		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		private SplineFollower Follower;
		private Animator Animator;
		void Start()					
		{
			Follower = GetComponent<SplineFollower>();
            Animator = GetComponent<Animator>();

			float animSpeed = Random.Range(0.2f, 0.7f);
			Animator.speed = animSpeed;
            Follower.followSpeed = animSpeed * 5f;
            Follower.Restart(Random.Range(0f, 0.1f));
			Follower.motion._offset.x = Random.Range(-1.5f, 1.5f);
			Follower.motion._offset.y = Random.Range(-1.5f, 1.5f);
			
        }

        void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

    }//end of class					
}//end of namespace					