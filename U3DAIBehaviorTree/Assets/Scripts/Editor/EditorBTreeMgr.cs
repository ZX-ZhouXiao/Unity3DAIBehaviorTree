using UnityEngine;
using UnityEditor;
using Game.AIBehaviorTree;
using System;
using System.IO;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using LitJson;

//	EditorBTreeMgr.cs
//	Author: Lu Zexi
//	2014-08-07




/// <summary>
/// Editor Behavior node manager
/// </summary>
public class EditorBTreeMgr
{
	public Dictionary<int,EditorBTree> m_mapTree = new Dictionary<int, EditorBTree>();

	private static EditorBTreeMgr s_cInstance;
	public static EditorBTreeMgr sInstance
	{
		get
		{
			if(s_cInstance == null)
			{
				s_cInstance = new EditorBTreeMgr();
			}
			return s_cInstance;
		}
	}

	public EditorBTreeMgr()
	{
		//
	}

	public void SaveEx()
	{
//		string filepath = EditorUtility.SaveFilePanel("Behavior Tree" , Application.dataPath , "","bytes");
//		BinaryWriter bw = new BinaryWriter(File.Create(filepath));
//		bw.Write(this.m_mapTree.Count);
//		foreach( KeyValuePair<int , EditorBTree> item in this.m_mapTree )
//		{
//			item.Value.WriteEx(bw);
//		}
//		bw.Close();
	}

	public void Save()
	{
		string filepath = EditorUtility.SaveFilePanel("Behavior Tree" , Application.dataPath , "","json");
		Debug.Log(filepath);
		JsonData data = new JsonData();
		data["trees"] = new JsonData();
		data["trees"].SetJsonType(JsonType.Array);
		foreach( KeyValuePair<int , EditorBTree> item in this.m_mapTree )
		{
			item.Value.WriteJson(data["trees"]);
		}
		File.WriteAllText(filepath,data.ToJson());
	}

	public void Load()
	{
		string filepath = EditorUtility.OpenFilePanel("Bahvior Tree" , Application.dataPath ,"json");
		if(filepath == "") return;
		this.m_mapTree.Clear();
		string txt = File.ReadAllText(filepath);
//		Debug.Log(txt);
//		JsonData json = JsonMapper.ToObject("{\"trees\":{\"id\":2,\"desc\":\"345_2\",\"node\":{\"id\":10,\"name\":\"Sequence_node\",\"node\":{\"id\":10,\"typeid\":0},\"typeid\":0,\"child\":}}}");
		JsonData json = JsonMapper.ToObject(txt);
		json = json["trees"];
		int count = json.Count;
		for(int i = 0 ; i<count ; i++)
		{
			EditorBTree bt = new EditorBTree();
			JsonData data = json[i];
			bt.ReadJson(data);
			this.m_mapTree.Add(bt.m_iID , bt);
		}
	}

	public EditorBNode GeneratorNode(int typeid)
	{
		EditorBNode node = null;
		node = new EditorBNode();
		node.m_cNode = BNodeFactory.sInstance.Create(typeid);
		node.m_cNode.SetID(node.m_iID);
		node.m_strName = node.m_cNode.GetName()+"_node";
		return node;
	}

	public EditorBTree GetTree( int id )
	{
		if( this.m_mapTree.ContainsKey(id))
			return this.m_mapTree[id];
		return null;
	}

	public List<EditorBTree> GetTrees()
	{
		List<EditorBTree> lst = new List<EditorBTree>();
		foreach( EditorBTree item in this.m_mapTree.Values )
			lst.Add(item);
		return lst;
	}

	public void Add( EditorBTree tree )
	{
		if( this.m_mapTree.ContainsKey(tree.m_iID))
		{
			Debug.LogError("The tree id is exist.");
			return;
		}
		this.m_mapTree.Add(tree.m_iID , tree);
	}

	public void Remove(EditorBTree tree)
	{
		if(tree == null ) return;
		if( this.m_mapTree.ContainsKey(tree.m_iID))
			this.m_mapTree.Remove(tree.m_iID);
		else
			Debug.LogError("The tree id is not exist.");
		return;
	}
}
