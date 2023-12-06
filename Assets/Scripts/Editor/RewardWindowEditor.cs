using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RewardWindowEditor : EditorWindow
    {
        #region INITIALIZATION
        // lists
        private List<RewardConfig> _rewards = new List<RewardConfig>();

        private RewardConfig _currentReward;
        private string _rewardName;
        private Sprite _rewardSprite;
        private int _rewardCatId;
        private List<CompositionTier> _rewardApparitionTiers = new List<CompositionTier>();
        private RewardPricing _rewardPricing;
        private float _rewardCost;
        
        private readonly List<string> _catsName = new List<string>();
    
        // window editor component
        private int _id;
        private Vector2 _sideBarScroll, _informationsScroll;
        private bool _canDisplayDetails = true;
        private RewardsConfig _rewardsConfig;
        #endregion
    
        [MenuItem("Integration Tools/Rewards")]
        static void InitializeWindow()
        {
            RewardWindowEditor window = GetWindow<RewardWindowEditor>();
            window.titleContent = new GUIContent("Rewards Integration");
            window.maxSize = new Vector2(801, 421);
            window.minSize = new Vector2(800, 420);
            window.Show();
        }
    
        private void OnEnable()
        {
            // initialize lists
            _rewards.Clear();
        
            // load the data from the rewards config
            LoadDataFromAsset();
        
            // display the first reward in the list if isn't empty
            if (_rewards.Count > 0)
            {
                UpdateInformations(_rewards[0]);
            }
            else
            {
                _canDisplayDetails = false;
            }
        }
    
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(175), GUILayout.ExpandHeight(true));
                {
                    _sideBarScroll = EditorGUILayout.BeginScrollView(_sideBarScroll);
                    {
                        DisplaySideBar();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            
                EditorGUILayout.BeginVertical();
                {
                    if (_canDisplayDetails)
                    {
                        _informationsScroll = EditorGUILayout.BeginScrollView(_informationsScroll);
                        {
                            DisplayInformations();
                        }
                        EditorGUILayout.EndScrollView();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    
        private void DisplaySideBar()
        {
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("REWARDS", EditorStyles.boldLabel);
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newInstance = CreateInstance<RewardConfig>();
                    newInstance.Initialize();
                    var date = DateTime.Now;
                    newInstance.id = "Reward_" + date.ToString("yyyyMMdd_HHmmss_fff");
        
                    _rewards.Add(newInstance);
                    UpdateInformations(newInstance);
                    _canDisplayDetails = true;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            // LIST OF CATS
            foreach (RewardConfig reward in _rewards)
            {
                string buttonName = reward.rewardName == "" ? "New reward" : reward.rewardName;
                if (GUILayout.Button($"{buttonName}", GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                {
                    UpdateInformations(reward);
                }
            }
        }
    
        private void DisplayInformations()
        {
            // INFORMATIONS
            GUILayout.Label("INFORMATION", EditorStyles.boldLabel);
            _rewardName = EditorGUILayout.TextField("Name", _rewardName);
            _rewardSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", _rewardSprite, typeof(Sprite), true);

            // REWARD
            GUILayout.Space(10);
            GUILayout.Label("REWARD", EditorStyles.boldLabel);
            DisplayRewardCat();
            DisplayApparitionTierChoser();
        
            // PRICING
            GUILayout.Space(10);
            GUILayout.Label("PRICING", EditorStyles.boldLabel);
            _rewardPricing = (RewardPricing)EditorGUILayout.EnumPopup("Pricing", _rewardPricing);
            if (_rewardPricing == RewardPricing.PAID) _rewardCost = EditorGUILayout.FloatField("Cost", _rewardCost);
            else _rewardCost = 0;
            
        
            // DATA MANAGEMENT
            // save button
            GUILayout.Space(20);
            if (GUILayout.Button("SAVE", GUILayout.ExpandWidth(true)))
            {
                SaveDataToAsset();
            }
    
            // delete button
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("DELETE", GUILayout.ExpandWidth(true)))
            {
                if (EditorUtility.DisplayDialog("Delete Reward", "Do you really want to permanently delete this reward?", "Yes", "No"))
                {
                    DeleteAssetData();
                }
            }
            GUI.backgroundColor = Color.white;
        }

        private void DisplayRewardCat()
        {
            foreach (EntityConfig cat in EditorMisc.FindEntitiesConfig().cats)
            {
                _catsName.Add(cat.entityName);
            }
            _rewardCatId = EditorGUILayout.Popup("Reward cat", _rewardCatId, _catsName.ToArray());
        }
    
        private void DisplayApparitionTierChoser()
        {
            GUILayout.BeginVertical("HelpBox");
            {
                // HEADER
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Apparition tiers");
                    if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                    {
                        _rewardApparitionTiers.Add(CompositionTier.SIMPLE);
                    }
                }
                GUILayout.EndHorizontal();

                for (int index = 0; index < _rewardApparitionTiers.Count; index++)
                {
                    DisplayApparitionTier(index);
                }
            }
            GUILayout.EndVertical();
        }

        private void DisplayApparitionTier(int index)
        {
            GUILayout.BeginHorizontal("HelpBox");
            {
                _rewardApparitionTiers[index] = (CompositionTier)EditorGUILayout.EnumPopup(_rewardApparitionTiers[index]);
                        
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (EditorUtility.DisplayDialog("Delete apparition tier", "Do you really want to permanently delete this apparition tier?", "Yes", "No"))
                    {
                        _rewardApparitionTiers.Remove(_rewardApparitionTiers[index]);
                    }
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
        
        private void UpdateInformations(RewardConfig rewardConfig)
        {
            _currentReward = rewardConfig;
        
            // BASE INFORMATIONS
            _rewardSprite = _currentReward.sprite;
            _rewardName = _currentReward.rewardName;
            _rewardCatId = _currentReward.rewardCatId;
            _rewardApparitionTiers = _currentReward.apparitionTiers;
            _rewardPricing = _currentReward.pricing;
            _rewardCost = _currentReward.cost;
        
            _canDisplayDetails = true;
        }
    
        private void SaveDataToAsset()
        {
            // exceptions
            if (_rewardName == "") return;
            
            // update data into current entity
            _currentReward.sprite = _rewardSprite;
            _currentReward.rewardName = _rewardName;
            _currentReward.rewardCatId = _rewardCatId;
            _currentReward.apparitionTiers =_rewardApparitionTiers;
            _currentReward.pricing = _rewardPricing;
            _currentReward.cost = _rewardCost;

            // get the path
            string path = $"Assets/Configs/Rewards/{_currentReward.id}.asset";

            // if the asset does not already exists then create a new one 
            if (!AssetDatabase.LoadAssetAtPath<RewardConfig>(path))
            {
                AssetDatabase.CreateAsset(_currentReward, path);
            }
        
            // save changes
            EditorUtility.SetDirty(_currentReward);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            // update rewards config
            UpdateRewardsConfig();
        }

        private void DeleteAssetData()
        {
            // remove from lists
            _rewards.Remove(_currentReward);

            // deleting the asset
            AssetDatabase.DeleteAsset($"Assets/Configs/Rewards/{_currentReward.id}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            // update rewards config
            UpdateRewardsConfig();
        }
    
        private void LoadDataFromAsset()
        {
            _rewardsConfig = EditorMisc.FindRewardsConfig();
            _rewards = _rewardsConfig.rewards;
        }

        private void UpdateRewardsConfig()
        {
            _rewardsConfig = EditorMisc.FindRewardsConfig();
            _rewardsConfig.rewards = _rewards;
            EditorUtility.SetDirty(_rewardsConfig);
        }
    }
}
