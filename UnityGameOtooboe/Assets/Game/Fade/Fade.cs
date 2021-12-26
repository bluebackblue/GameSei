

/** Game.Fade
*/
namespace Game.Fade
{
	/** Fade
	*/
	public sealed class Fade : System.IDisposable
	{
		/** Mode
		*/
		public enum Mode
		{
			Out,
			In,
			FadeIn,
			FadeOut,
		}

		/** mode
		*/
		public Mode mode;

		/** Item
		*/
		public struct Item
		{
			public BlueBack.Gl.SpriteIndex sprite;
			public float size;
			public float time;
		}

		/** sprite
		*/
		public Item[] list;


		/** fade_gameobject
		*/
		public UnityEngine.GameObject fade_gameobject;
		public Fade_MonoBehaviour fade_monobehaviour;

		/** time
		*/
		public float time;

		/** constructor
		*/
		public Fade()
		{
			//fade_gameobject
			this.fade_gameobject = new UnityEngine.GameObject("fade");
			UnityEngine.GameObject.DontDestroyOnLoad(this.fade_gameobject);
			this.fade_monobehaviour = this.fade_gameobject.AddComponent<Fade_MonoBehaviour>();
			this.fade_monobehaviour.fade = this;
			this.fade_monobehaviour.visible = false;

			//mode
			this.mode = Mode.In;

			//sprite
			this.list = new Item[20 * 12];
			for(int ii = 0;ii < this.list.Length;ii++){
				this.list[ii].sprite = Execute.Engine.GetSingleton().gl.spritelist[2].CreateSprite(false,(int)UnitySetting.MaterialIndex.Transparent,(int)UnitySetting.TextureIndex.None,UnityEngine.Color.black,0,0,0,0,UnitySetting.Config.SCREEN_W,UnitySetting.Config.SCREEN_H);
			}
		}

		/** [System.IDisposable]Dispose
		*/
		public void Dispose()
		{
			if(this.fade_gameobject != null){
				UnityEngine.GameObject.Destroy(this.fade_gameobject);
				this.fade_gameobject = null;
			}
		}

		/** SetVisible

			return == true : �����B

		*/
		public bool SetVisible(bool a_flag)
		{
			this.fade_monobehaviour.visible = a_flag;

			if(a_flag == true){
				if(this.mode == Mode.Out){
					return true;
				}
			}else{
				if(this.mode == Mode.In){
					return true;
				}
			}

			return false;
		}

		/** UnityUpdate
		*/
		public void UnityUpdate()
		{
			switch(this.mode){
			case Mode.Out:
				{
					if(this.fade_monobehaviour.visible == false){
						this.time = 1.0f;
						this.mode = Mode.FadeIn;

						for(int ii = 0;ii < this.list.Length;ii++){
							this.list[ii].time = UnityEngine.Random.value;
						}
					}
				}break;
			case Mode.In:
				{
					if(this.fade_monobehaviour.visible == true){
						this.time = 0.0f;
						this.mode = Mode.FadeOut;

						for(int ii = 0;ii < this.list.Length;ii++){
							this.list[ii].size = 64 + UnityEngine.Random.value * 64;
							this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index].color = new UnityEngine.Color(UnityEngine.Random.value,UnityEngine.Random.value,UnityEngine.Random.value,1.0f);
							this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index].visible = false;
							this.list[ii].time = UnityEngine.Random.value;

							int t_x = ii % 20;
							int t_y  = ii / 20;
							BlueBack.Gl.SpriteTool.SetXYWH(ref this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index],t_x * 64,t_y * 64,(int)this.list[ii].size,(int)this.list[ii].size,UnitySetting.Config.SCREEN_W,UnitySetting.Config.SCREEN_H);
						}
					}
				}break;
			case Mode.FadeIn:
				{
					this.time -= UnityEngine.Mathf.Clamp01(UnityEngine.Time.deltaTime * 3.0f);

					for(int ii = 0;ii < this.list.Length;ii++){
						if(this.list[ii].time <= this.time){
							this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index].visible = true;
						}else{
							this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index].visible = false;
						}
					}
					
					if(this.time <= 0.0f){
						for(int ii = 0;ii < this.list.Length;ii++){
							this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index].visible = false;
						}
						this.mode = Mode.In;
					}
				}
				break;
			case Mode.FadeOut:
				{
					this.time += UnityEngine.Mathf.Clamp01(UnityEngine.Time.deltaTime * 3.0f);

					for(int ii = 0;ii < this.list.Length;ii++){
						if(this.list[ii].time <= this.time){
							this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index].visible = true;
						}else{
							this.list[ii].sprite.spritelist.buffer[this.list[ii].sprite.index].visible = false;
						}
					}

					if(this.time >= 1.0f) {
						this.mode = Mode.Out;
					}
				}break;
			}
		}
	}
}
