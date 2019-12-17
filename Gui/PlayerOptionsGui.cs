using UnityEngine;
using PortiaHelper.Core;

namespace PortiaHelper.Gui
{
	public class PlayerOptionsGui : MonoBehaviour
	{
		protected Rect _windowRect;

		protected string _expRatioStr;

		protected string _goldRatioStr;

		protected string _expInfo;

		protected string _goldInfo;

		void Awake() {
			_windowRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 300);

			_expRatioStr = Central.Instance.PlayerOptions.ExpRatio.ToString();
			_goldRatioStr = Central.Instance.PlayerOptions.GoldRatio.ToString();
		}

		void OnEnable() {
			BuildExpInfo();
			BuildGoldInfo();

			Central.Instance.PauseGame();
		}

		void OnDisable() {
			Central.Instance.ResumeGame();
		}

		void OnGUI() {
			GUILayout.Window(1, _windowRect, DrawWindow, "Player Options");
		}

		void DrawWindow(int windowId) {
			Central.Instance.PlayerOptions.GodMode = GUILayout.Toggle(Central.Instance.PlayerOptions.GodMode, "God Mode");
			Central.Instance.PlayerOptions.InfiniteStamina = GUILayout.Toggle(Central.Instance.PlayerOptions.InfiniteStamina, "Infinite Stamina");
			Central.Instance.PlayerOptions.InfiniteVP = GUILayout.Toggle(Central.Instance.PlayerOptions.InfiniteVP, "Infinite VP");
			Central.Instance.PlayerOptions.NoDrop = GUILayout.Toggle(Central.Instance.PlayerOptions.NoDrop, "No Drops");

			// Exp Ratio
			GUILayout.BeginVertical(GUILayout.Height(40f));
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(_expRatioStr);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();

				var newExpRatio = GUILayout.HorizontalSlider(Central.Instance.PlayerOptions.ExpRatio, 3f, 50f);

				if (newExpRatio != Central.Instance.PlayerOptions.ExpRatio) {
					Central.Instance.PlayerOptions.ExpRatio = Mathf.Round(newExpRatio);
					BuildExpInfo();
				}

				GUILayout.Label(_expInfo);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();

			// Gold Ratio
			GUILayout.BeginVertical(GUILayout.Height(40f));
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(_goldRatioStr);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();

				var newGoldRatio = GUILayout.HorizontalSlider(Central.Instance.PlayerOptions.GoldRatio, 50f, 1_000f);

				if (newGoldRatio != Central.Instance.PlayerOptions.GoldRatio) {
					Central.Instance.PlayerOptions.GoldRatio = Mathf.Round(newGoldRatio);
					BuildGoldInfo();
				}

				GUILayout.Label(_goldInfo);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}

		void BuildExpInfo() {
			_expInfo = $"You'll receive {Mathf.RoundToInt(100 * Central.Instance.PlayerOptions.ExpRatio)} exp for an action that gives you 100 exp.";
			_expRatioStr = Central.Instance.PlayerOptions.ExpRatio.ToString();
		}

		void BuildGoldInfo() {
			_goldInfo = $"You'll receive {Mathf.RoundToInt(100 * Central.Instance.PlayerOptions.GoldRatio)} golds for an action that gives you 100 golds.";
			_goldRatioStr = Central.Instance.PlayerOptions.GoldRatio.ToString();
		}
	}
}
