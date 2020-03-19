using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Solis
{
    public class SolisContentManager : ContentManager
    {
        private Dictionary<string, Effect> _loadedEffects = new Dictionary<string, Effect>();

        private List<IDisposable> _disposableAssets;

        private List<IDisposable> DisposableAssets
        {
            get
            {
                if (_disposableAssets == null)
                {
                    _disposableAssets = new List<IDisposable>();
                }
                return _disposableAssets;
            }
        }

        public SolisContentManager(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory)
        { }

        public SolisContentManager(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        public SolisContentManager() : base(((Game)SolisCore.Instance).Services, ((Game)SolisCore.Instance).Content.RootDirectory)
        { }

        #region Loaders

        /// <summary>
        /// loads a texture2D from xnb or from png/jpg. Xnb should not have extension while later should.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="premultiplyAlpha"></param>
        /// <returns></returns>
        public Texture2D LoadTexture(string name, bool premultiplyAlpha = false)
        {
            //if no extension assume an xnb
            if (string.IsNullOrEmpty(Path.GetExtension(name)))
            {
                var result = Load<Texture2D>(name);
                return result;
                //return Load<Texture2D>(name);
            }

            if (LoadedAssets.TryGetValue(name, out var asset))
            {
                if (asset is Texture2D tex)
                    return tex;
            }

            using (var stream = Path.IsPathRooted(name) ? File.OpenRead(name) : TitleContainer.OpenStream(name))
            {
                var texture = Texture2D.FromStream(SolisCore.Instance.GraphicsDevice, stream);
                texture.Name = name;
                LoadedAssets[name] = texture;
                DisposableAssets.Add(texture);

                return texture;
            }
        }

        /// <summary>
        /// Load a sound effect from xnb or wav
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SoundEffect LoadSoundEffect(string name)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(name)))
                return Load<SoundEffect>(name);
            if (LoadedAssets.TryGetValue(name, out var asset))
            {
                if (asset is SoundEffect sfx)
                {
                    return sfx;
                }
            }

            using (var stream = Path.IsPathRooted(name) ? File.OpenRead(name) : TitleContainer.OpenStream(name))
            {
                var sfx = SoundEffect.FromStream(stream);
                LoadedAssets[name] = sfx;
                DisposableAssets.Add(sfx);
                return sfx;
            }
        }

        #endregion Loaders

        #region async loaders

        public void LoadAsyncTexture(string assetName, Action<Texture2D> onLoaded = null)
        {
            var syncContext = SynchronizationContext.Current;

            Task loadTask = new Task(() =>
            {
                var asset = Load<Texture2D>(assetName);
                //if we have a callback do it on main thread
                onLoaded?.Invoke(asset);
            });
            loadTask.Start();
        }

        public void LoadAsyncTextures(string[] assetNames, Action onLoaded = null)
        {
            var syncContext = SynchronizationContext.Current;
            Task loadTask = new Task(() =>
            {
                for (var i = 0; i < assetNames.Length; i++)
                {
                    var asset = Load<Texture2D>(assetNames[i]);
                    Console.WriteLine("Loaded asset {0}", assetNames[i]);
                }
                onLoaded?.Invoke();
            });
            loadTask.Start();
        }

        public void LoadAsyncSong(string assetName, Action<Song> onLoaded = null)
        {
            var syncContext = SynchronizationContext.Current;

            Task loadTask = new Task(() =>
            {
                var asset = Load<Song>(@"Sound\" + assetName);
                Console.WriteLine("Loaded asset {0}", assetName);
                onLoaded?.Invoke(asset);
            });
            loadTask.Start();
        }

        #endregion async loaders

        public void UnloadAsset<T>(string assetName) where T : class, IDisposable
        {
            if (IsAssetLoaded(assetName))
            {
                try
                {
                    var assetToRemove = LoadedAssets[assetName];
                    for (var i = 0; i < DisposableAssets.Count; i++)
                    {
                        var typedAsset = DisposableAssets[i] as T;
                        if (typedAsset != null && typedAsset == assetToRemove)
                        {
                            typedAsset.Dispose();
                            DisposableAssets.RemoveAt(i);
                            break;
                        }
                    }

                    LoadedAssets.Remove(assetName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cound not unload asset {0}. {1}", assetName, e);
                }
            }
        }

        public bool UnloadEffect(string effectName)
        {
            if (_loadedEffects.ContainsKey(effectName))
            {
                _loadedEffects[effectName].Dispose();
                _loadedEffects.Remove(effectName);
                return true;
            }

            return false;
        }

        public string GetPathForLoadedAsset(IDisposable asset)
        {
            if (LoadedAssets.ContainsValue(asset))
            {
                foreach (var kv in LoadedAssets)
                {
                    if (kv.Value == asset)
                        return kv.Key;
                }
            }

            return null;
        }

        //public override void Unload()
        //{
        //    base.Unload();
        //
        //    foreach (var key in _loadedEffects.Keys)
        //        _loadedEffects[key].Dispose();
        //    foreach (var asset in _loadedAssets.Keys)
        //        _loadedAssets[asset].Dispose();
        //    _loadedAssets.Clear();
        //
        //    _loadedEffects.Clear();
        //     Dispose();
        //    GC.Collect();
        //}

        public bool UnloadEffect(Effect effect) => UnloadEffect(effect.Name);

        public bool IsAssetLoaded(string assetName) => LoadedAssets.ContainsKey(assetName);

        public Texture2D GetAssetTexture(string name)
        {
            var asset = LoadedAssets[name] as Texture2D;
            return asset;
        }

        public Song GetAssetSong(string name)
        {
            var asset = LoadedAssets["Sound/" + name] as Song;
            return asset;
        }

        public int AssetCount()
        {
            return LoadedAssets.Count;
        }
    }
}