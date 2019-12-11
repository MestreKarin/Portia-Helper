using UnityEngine;
using Pathea;
using Pathea.ActorNs;
using Pathea.ModuleNs;

namespace PortiaHelper.Modules
{
	public class InfiniteVp : MonoBehaviour
	{
		private Player _player;

		public void Start() {
			_player = Module<Player>.Self;
		}

		public void Update() {
			if (_player.actor != null) {				
				_player.actor.Vp = 100.0f;

				if (_player.actor.CurRidableActor != null) {
					_player.actor.CurRidableActor.Vp = 100.0f;
				}
			}
		}
	}
}
