/*
 * Copyright (c) 2019 Dummiesman
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Dummiesman;

namespace Dummiesman
{
    public enum SplitMode
    {
        None,
        Object,
        Material
    }

    public abstract class MeshLoader
    {

        public StreamFactory streamFactory;
        //options
        /// <summary>
        /// Determines how objects will be created
        /// </summary>
        public SplitMode SplitMode = SplitMode.Object;

        //global lists, accessed by objobjectbuilder
        internal List<Vector3> Vertices = new List<Vector3>();
        internal List<Vector3> Normals = new List<Vector3>();
        internal List<Vector2> UVs = new List<Vector2>();

        //materials, accessed by objobjectbuilder
        internal Dictionary<string, Material> Materials;

        //file info for files loaded from file path, used for GameObject naming and MTL finding
        //private FileInfo _objInfo;

        public MeshLoader(StreamFactory streamFactory)
        {
            this.streamFactory = streamFactory;
        }

        /// <summary>
        /// Helper function to load mtllib statements
        /// </summary>
        /// <param name="mtlLibPath"></param>
        protected void LoadMaterialLibrary(string mtlLibPath)
        {
            if (streamFactory.Exists(mtlLibPath))
            {
                Materials = new MTLLoader(streamFactory).Load(streamFactory.OpenStream(mtlLibPath));
                return;
            }
        }


        protected GameObject BuildGameObject(Dictionary<String, OBJObjectBuilder> builderDict)
        {

            List<GameObject> objObjects = new List<GameObject>();
            foreach (var builder in builderDict)
            {

                if (builder.Value.PushedFaceCount == 0)
                    continue;

                var builtObj = builder.Value.Build();

                objObjects.Add(builtObj);
            }

            GameObject obj = new GameObject(/*_objInfo != null ? Path.GetFileNameWithoutExtension(_objInfo.Name) :*/ "WavefrontObject");
            obj.transform.localScale = new Vector3(-0.001f, 0.001f, 0.001f);

            foreach (var g in objObjects)
            {
                g.transform.SetParent(obj.transform, false);
            }

            ObjBox box = CalculateBigBox(builderDict);
            ObjBoxComponent.FromGameObject(obj, box);

            return obj;
        }

        protected ObjBox CalculateBigBox(Dictionary<String, OBJObjectBuilder> builderDict)
        {
            List<ObjBox> boxes = CalculateBoxOfChild(builderDict);
            ObjBox box = ObjBox.FromBoxes(boxes);
            return box;
        }

        protected void ResetOriginBottom(Dictionary<String, OBJObjectBuilder> builderDict)
        {
            List<ObjBox> boxes = CalculateBoxOfChild(builderDict);
            Vector3 bottomOrigin = FindBottomOrigin(boxes);

            TranslateVertices(builderDict, -bottomOrigin);
        }


        protected List<ObjBox> CalculateBoxOfChild(Dictionary<String, OBJObjectBuilder> builderDict)
        {
            List<ObjBox> boxes = new List<ObjBox>();
            foreach (var builder in builderDict)
            {
                if (builder.Value.PushedFaceCount == 0)
                    continue;
                boxes.Add(builder.Value.Box);
            }
            return boxes;
        }

        protected Vector3 FindBottomOrigin(List<ObjBox> boxes)
        {
            Vector3 calculatedOrigin = Vector3.zero;
            float minY = float.MaxValue;
            foreach (var objBox in boxes)
            {
                calculatedOrigin += objBox.center;
                if (minY > objBox.minExtend.y)
                    minY = objBox.minExtend.y;
            }

            calculatedOrigin /= boxes.Count;
            calculatedOrigin = new Vector3(calculatedOrigin.x, minY, calculatedOrigin.z);
            return calculatedOrigin;
        }



        public void TranslateVertices(Dictionary<String, OBJObjectBuilder> builderDict, Vector3 translate)
        {
            foreach (var builder in builderDict)
            {
                if (builder.Value.PushedFaceCount == 0)
                    continue;
                builder.Value.TranslateVectices(translate);
                builder.Value.Box.Translate(translate);

            }
        }

        /// <summary>
        /// Load an OBJ file from a stream. No materials will be loaded, and will instead be supplemented by a blank white material.
        /// </summary>
        /// <param name="input">Input OBJ stream</param>
        /// <returns>Returns a GameObject represeting the OBJ file, with each imported object as a child.</returns>
        public GameObject Load(Stream input)
        {
            var reader = new StreamReader(input);
            //var reader = new StringReader(inputReader.ReadToEnd());

            Dictionary<string, OBJObjectBuilder> builderDict = new Dictionary<string, OBJObjectBuilder>();

            builderDict = LoadBuilderDictionaries(reader);

            ResetOriginBottom(builderDict);

            //finally, put it all together
            GameObject obj = BuildGameObject(builderDict);

            return obj;
        }

        internal abstract Dictionary<string, OBJObjectBuilder> LoadBuilderDictionaries(StreamReader reader);

        /// <summary>
        /// Load an OBJ and MTL file from a stream.
        /// </summary>
        /// <param name="input">Input OBJ stream</param>
        /// /// <param name="mtlInput">Input MTL stream</param>
        /// <returns>Returns a GameObject represeting the OBJ file, with each imported object as a child.</returns>
        public GameObject Load(Stream input, Stream mtlInput)
        {
            if (streamFactory == null)
            {
                streamFactory = new FileStreamFactory();
            }

            var mtlLoader = new MTLLoader(streamFactory);
            Materials = mtlLoader.Load(mtlInput);

            return Load(input);
        }

        /// <summary>
        /// Load an OBJ and MTL file from a file path.
        /// </summary>
        /// <param name="path">Input OBJ path</param>
        /// /// <param name="mtlPath">Input MTL path</param>
        /// <returns>Returns a GameObject represeting the OBJ file, with each imported object as a child.</returns>
        public GameObject Load(string path, string mtlPath)
        {

            //_objInfo = new FileInfo(path);
            if (mtlPath != null && streamFactory.Exists(mtlPath))
            {
                var mtlLoader = new MTLLoader(streamFactory);
                Materials = mtlLoader.Load(mtlPath);
            }

            using (var fs = streamFactory.OpenStream(path))
            {
                return Load(fs);
            }

        }

        /// <summary>
        /// Load an OBJ file from a file path. This function will also attempt to load the MTL defined in the OBJ file.
        /// </summary>
        /// <param name="path">Input OBJ path</param>
        /// <returns>Returns a GameObject represeting the OBJ file, with each imported object as a child.</returns>
        public GameObject Load(string path)
        {
            return Load(path, null);
        }
    }
}
