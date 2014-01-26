
/*
-----------------------------------------------------------------------------
This source file is part of mogre-procedural
For the latest info, see http://code.google.com/p/mogre-procedural/
my blog:http://hi.baidu.com/rainssoft
this is overwrite  ogre-procedural c++ project using c#, look  ogre-procedural c++ source http://code.google.com/p/ogre-procedural/
   
Copyright (c) 2013-2020 rains soft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
-----------------------------------------------------------------------------
*/

//#ifndef PROCEDURAL_SVG_INCLUDED
#define PROCEDURAL_SVG_INCLUDED

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;

    public class SvgLoader
    {

        uint mNumSeg;

        /** Internal class to parse path element */
        internal class SvgLoaderPath
        {
            //private:
            int index;
            std_vector<string> parts;//=new std_vector<string>();
            std_vector<Vector2> curve = new std_vector<Vector2>();
            Vector2 point;
            uint mNumSeg;


            public Shape shape = new Shape();
            public float px;
            public float py;

            // SvgLoaderPath(std::vector<std::string> p, unsigned int ns);
            public SvgLoaderPath(std_vector<string> p, uint ns) {
                parts = p;
                mNumSeg = ns;
                px = 0.0f;
                py = 0.0f;
                index = 0;
                char lastCmd = (char)0;

                while (index < p.Count) {
                    try {
                        char newCmd = parts[index][0];
                        bool next = true;
                        if (lastCmd != newCmd && newCmd != '.' && newCmd != '-' && (newCmd < '0' || newCmd > '9') && curve.size() > 3 && ((lastCmd == 'c' || lastCmd == 'C') && (newCmd == 's' || newCmd == 'S') || (lastCmd == 'q' || lastCmd == 'Q') && (newCmd == 't' || newCmd == 'T'))) {
                            // finish curve
                            finishCurve(lastCmd);
                        }
                        switch (newCmd) {
                            case 'l':
                                parseLineTo(true, next);
                                break;
                            case 'L':
                                parseLineTo(false, next);
                                break;
                            case 'm':
                                parseMoveTo(true, next);
                                newCmd = 'l';
                                break;
                            case 'M':
                                parseMoveTo(false, next);
                                newCmd = 'L';
                                break;
                            case 'h':
                                parseHLineTo(true, next);
                                break;
                            case 'H':
                                parseHLineTo(false, next);
                                break;
                            case 'v':
                                parseVLineTo(true, next);
                                break;
                            case 'V':
                                parseVLineTo(false, next);
                                break;
                            case 'c':
                                curve.push_back(point);
                                parseCurveCTo(true, next);
                                break;
                            case 'C':
                                curve.push_back(point);
                                parseCurveCTo(false, next);
                                break;
                            case 's':
                                parseCurveSTo(true, next);
                                break;
                            case 'S':
                                parseCurveSTo(false, next);
                                break;
                            case 'q':
                                curve.push_back(point);
                                parseCurveQTo(true, next);
                                break;
                            case 'Q':
                                curve.push_back(point);
                                parseCurveQTo(false, next);
                                break;
                            case 't':
                                parseCurveTTo(true, next);
                                break;
                            case 'T':
                                parseCurveTTo(false, next);
                                break;
                            case 'a':
                                parseArcTo(true, next);
                                break;
                            case 'A':
                                parseArcTo(false, next);
                                break;
                            case 'z':
                            case 'Z':
                                shape.close();
                                index++;
                                break;
                            default:
                                newCmd = lastCmd;
                                next = false;
                                switch (lastCmd) {
                                    case 'l':
                                        parseLineTo(true, next);
                                        break;
                                    case 'L':
                                        parseLineTo(false, next);
                                        break;
                                    case 'm':
                                        parseMoveTo(true, next);
                                        break;
                                    case 'M':
                                        parseMoveTo(false, next);
                                        break;
                                    case 'h':
                                        parseHLineTo(true, next);
                                        break;
                                    case 'H':
                                        parseHLineTo(false, next);
                                        break;
                                    case 'v':
                                        parseVLineTo(true, next);
                                        break;
                                    case 'V':
                                        parseVLineTo(false, next);
                                        break;
                                    case 'c':
                                        parseCurveCTo(true, next);
                                        break;
                                    case 'C':
                                        parseCurveCTo(false, next);
                                        break;
                                    case 's':
                                        parseCurveSTo(true, next);
                                        break;
                                    case 'S':
                                        parseCurveSTo(false, next);
                                        break;
                                    case 'q':
                                        parseCurveQTo(true, next);
                                        break;
                                    case 'Q':
                                        parseCurveQTo(false, next);
                                        break;
                                    case 't':
                                        parseCurveTTo(true, next);
                                        break;
                                    case 'T':
                                        parseCurveTTo(false, next);
                                        break;
                                    case 'a':
                                        parseArcTo(true, next);
                                        break;
                                    case 'A':
                                        parseArcTo(false, next);
                                        break;

                                    default:
                                        break;
                                }
                                break;
                        }
                        lastCmd = newCmd;
                    }
                    catch {
                    }
                }
                if (curve.size() > 0)
                    finishCurve(lastCmd);
            }


            internal Shape getSvgShape() {
                shape.translate(px, py);
                return shape;
            }

            internal bool isValid() {
                return shape.getPoints().Length > 2;
            }



            internal float CalculateVectorAngle(float ux, float uy, float vx, float vy) {
                double ta = atan2(uy, ux);
                double tb = atan2(vy, vx);

                if (tb >= ta)
                    return (float)(tb - ta);

                return Math.TWO_PI - (float)(ta - tb);
            }


            void parseArcTo(bool rel, bool next) {
                if (next) index++;
                float rx = 0.0f;
                if (!parseReal(ref rx))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float ry = 0.0f;
                if (!parseReal(ref ry))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float x_axis_rotation = 0.0f;
                if (!parseReal(ref x_axis_rotation))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float large_arc_flag = 0.0f;
                if (!parseReal(ref large_arc_flag))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float sweep_flag = 0.0f;
                if (!parseReal(ref sweep_flag))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float x = 0.0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float y = 0.0f;
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");

                float RadiansPerDegree = Math.PI / 180.0f;
                float epx = rel ? point.x + x : x;
                float epy = rel ? point.y + y : y;
                bool largeArc = (large_arc_flag > 0);
                bool clockwise = (sweep_flag > 0);

                if (epx == point.x && epy == point.y)
                    return;

                if (rx == 0.0f && ry == 0.0f) {
                    point = new Vector2(epx, epy);
                    shape.addPoint(point);
                    return;
                }

                float sinPhi = sin(x_axis_rotation * RadiansPerDegree);
                float cosPhi = cos(x_axis_rotation * RadiansPerDegree);

                float x1dash = cosPhi * (point.x - epx) / 2.0f + sinPhi * (point.y - epy) / 2.0f;
                float y1dash = -sinPhi * (point.x - epx) / 2.0f + cosPhi * (point.y - epy) / 2.0f;

                float root;
                float numerator = rx * rx * ry * ry - rx * rx * y1dash * y1dash - ry * ry * x1dash * x1dash;

                if (numerator < 0.0) {
                    float s = (float)sqrt(1.0f - numerator / (rx * rx * ry * ry));

                    rx *= s;
                    ry *= s;
                    root = 0.0f;
                }
                else {
                    root = ((largeArc && clockwise) || (!largeArc && !clockwise) ? -1.0f : 1.0f) * sqrt(numerator / (rx * rx * y1dash * y1dash + ry * ry * x1dash * x1dash));
                }

                float cxdash = root * rx * y1dash / ry;
                float cydash = -root * ry * x1dash / rx;

                float cx = cosPhi * cxdash - sinPhi * cydash + (point.x + epx) / 2.0f;
                float cy = sinPhi * cxdash + cosPhi * cydash + (point.y + epy) / 2.0f;

                float theta1 = CalculateVectorAngle(1.0f, 0.0f, (x1dash - cxdash) / rx, (y1dash - cydash) / ry);
                float dtheta = CalculateVectorAngle((x1dash - cxdash) / rx, (y1dash - cydash) / ry, (-x1dash - cxdash) / rx, (-y1dash - cydash) / ry);

                if (!clockwise && dtheta > 0)
                    dtheta -= 2.0f * Math.PI;
                else if (clockwise && dtheta < 0)
                    dtheta += 2.0f * Math.PI;

                int segments = (int)ceil((double)abs(dtheta / (Math.PI / 2.0f)));
                float delta = dtheta / segments;
                float t = 8.0f / 3.0f * sin(delta / 4.0f) * sin(delta / 4.0f) / sin(delta / 2.0f);

                float startX = point.x;
                float startY = point.y;

                BezierCurve2 bezier = new BezierCurve2();
                bezier.addPoint(startX, startY);
                for (int i = 0; i < segments; ++i) {
                    float cosTheta1 = cos(theta1);
                    float sinTheta1 = sin(theta1);
                    float theta2 = theta1 + delta;
                    float cosTheta2 = cos(theta2);
                    float sinTheta2 = sin(theta2);

                    float endpointX = cosPhi * rx * cosTheta2 - sinPhi * ry * sinTheta2 + cx;
                    float endpointY = sinPhi * rx * cosTheta2 + cosPhi * ry * sinTheta2 + cy;

                    float dx1 = t * (-cosPhi * rx * sinTheta1 - sinPhi * ry * cosTheta1);
                    float dy1 = t * (-sinPhi * rx * sinTheta1 + cosPhi * ry * cosTheta1);

                    float dxe = t * (cosPhi * rx * sinTheta2 + sinPhi * ry * cosTheta2);
                    float dye = t * (sinPhi * rx * sinTheta2 - cosPhi * ry * cosTheta2);

                    bezier.addPoint(startX + dx1, startY + dy1);
                    bezier.addPoint(endpointX + dxe, endpointY + dye);

                    theta1 = theta2;
                    startX = endpointX;
                    startY = endpointY;
                }
                point = new Vector2(epx, epy);
                bezier.addPoint(point);
                bezier.setNumSeg(mNumSeg);
                std_vector<Vector2> pointList = bezier.realizeShape().getPointsReference();//getPoints();
                Vector2 lp = shape.getPoint(shape.getPoints().Length - 1);
                //for (std::vector<Vector2>::iterator iter = pointList.begin(); iter != pointList.end(); iter++)
                for (int ii = 0; ii < pointList.size(); ii++) {
                    //if (iter == pointList.begin())
                    if (ii == 0) {
                        //if (*iter != lp) shape.addPoint(*iter);
                        if (pointList[ii] != lp) shape.addPoint(pointList[ii]);
                    }
                    else
                        shape.addPoint(pointList[ii]);//shape.addPoint(*iter);
                }
            }


            //-----------------------------------------------------------------------
            internal void finishCurve(char lc) {
                int n;
                if (lc == 'c' || lc == 'C' || lc == 's' || lc == 'S')
                    n = 3;
                else if (lc == 'q' || lc == 'Q' || lc == 't' || lc == 'T')
                    n = 2;
                else
                    n = curve.size() - 1;

                for (int i = 0; i < curve.size(); i += n) {
                    if (i + 3 >= curve.size()) break;
                    BezierCurve2 bc2 = new BezierCurve2();
                    bc2.setNumSeg(mNumSeg);
                    bc2.addPoint(curve[i + 0]);
                    bc2.addPoint(curve[i + 1]);
                    bc2.addPoint(curve[i + 2]);
                    bc2.addPoint(curve[i + 3]);
                    Shape bc2shape = bc2.realizeShape();
                    //Vector2 lp = shape.getPoint(shape.getPoints().size() - 1);
                    Vector2 lp = shape.getPoint(shape.getPointCount() - 1);
                    //for (std::vector<Vector2>::iterator iter = bc2shape.getPoints().begin(); iter != bc2shape.getPoints().end(); iter++)
                    for (int j = 0; j < bc2shape.getPointCount(); j++) {
                        //if (iter == bc2shape.getPoints().begin())
                        if (j == 0) {
                            //if (*iter != lp) shape.addPoint(*iter);
                            if (bc2shape.getPointsReference()[j] != lp) shape.addPoint(bc2shape.getPointsReference()[j]);
                        }
                        else
                            shape.addPoint(bc2shape.getPointsReference()[j]);//shape.addPoint(*iter);
                    }
                }
                curve.clear();
            }
            //-----------------------------------------------------------------------
            private bool parseReal(ref float var) {
                return parseReal(ref var, 0.0f);
            }
           //
            //ORIGINAL LINE: inline bool parseReal(Ogre::Real* var, Ogre::float defaultReal = 0.0f)
            private bool parseReal(ref float var, float defaultReal) {
                if (var == null)
                    return false;
                if (index >= parts.Count)
                    return false;
                try {
                    var = float.Parse(parts[index]);
                    index++;
                }
                catch {
                    return false;
                }
                return true;
            }
            //-----------------------------------------------------------------------        
            void parseCurveSTo(bool rel, bool next) {
                if (next) index++;
                Vector2 offset = Vector2.ZERO;
                if (rel) offset = point;

                float x1 = point.x;
                float y1 = point.y;
                if (curve.size() > 2) {
                    Vector2 mirror = curve[curve.size() - 2];
                    Vector2 diff = mirror - point;
                    x1 = -1.0f * diff.x + point.x;
                    y1 = -1.0f * diff.y + point.y;
                }
                float x2 = 0.0f;
                if (!parseReal(ref x2))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float y2 = 0.0f;
                if (!parseReal(ref y2))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float x = 0.0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                float y = 0.0f;
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveSTo");
                curve.push_back(new Vector2(x1, y1) + offset);
                curve.push_back(new Vector2(x2, y2) + offset);
                point = new Vector2(x, y) + offset;
                curve.push_back(point);
            }
            //-----------------------------------------------------------------------	
            void parseCurveQTo(bool rel, bool next) {
                if (next) index++;
                Vector2 offset = Vector2.ZERO;
                if (rel) offset = point;

                float x1 = 0.0f;
                if (!parseReal(ref x1))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveQTo");
                float y1 = 0.0f;
                if (!parseReal(ref y1))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveQTo");
                float x = 0.0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveQTo");
                float y = 0.0f;
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveQTo");
                curve.push_back(new Vector2(x1, y1) + offset);
                point = new Vector2(x, y) + offset;
                curve.push_back(point);
            }
            //-----------------------------------------------------------------------
            void parseCurveTTo(bool rel, bool next) {
                if (next) index++;
                Vector2 offset = Vector2.ZERO;
                if (rel) offset = point;

                float x1 = point.x;
                float y1 = point.y;
                if (curve.size() > 2) {
                    Vector2 mirror = curve[curve.size() - 2];
                    Vector2 diff = mirror - point;
                    x1 = -1.0f * diff.x + point.x;
                    y1 = -1.0f * diff.y + point.y;
                }
                float x = 0.0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveTTo");
                float y = 0.0f;
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveTTo");
                curve.push_back(new Vector2(x1, y1) + offset);
                point = new Vector2(x, y) + offset;
                curve.push_back(point);
            }
            //----------------------------------------------------------------------- 
            void parseCurveCTo(bool rel, bool next) {
                if (next) index++;
                Vector2 offset = Vector2.ZERO;
                if (rel) offset = point;

                float x1 = 0.0f;
                if (!parseReal(ref x1))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveCTo");
                float y1 = 0.0f;
                if (!parseReal(ref y1))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveCTo");
                float x2 = 0.0f;
                if (!parseReal(ref x2))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveCTo");
                float y2 = 0.0f;
                if (!parseReal(ref y2))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveCTo");
                float x = 0.0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveCTo");
                float y = 0.0f;
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseCurveCTo");
                curve.push_back(new Vector2(x1, y1) + offset);
                curve.push_back(new Vector2(x2, y2) + offset);
                point = new Vector2(x, y) + offset;
                curve.push_back(point);
            }
            //-----------------------------------------------------------------------  

            //private:
            internal void parseMoveTo(bool rel, bool next) {
                if (next) index++;
                float x = 0f, y = 0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseMoveTo");
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseMoveTo");
                point = new Vector2(x, y);
                shape.addPoint(point);
            }
            //-----------------------------------------------------------------------

            internal void parseLineTo(bool rel, bool next) {
                if (next) index++;
                float x = 0.0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseLineTo");
                float y = 0.0f;
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseLineTo");
                if (rel)
                    point = new Vector2(point.x + x, point.y + y);
                else
                    point = new Vector2(x, y);
                shape.addPoint(point);
            }

            internal void parseHLineTo(bool rel, bool next) {
                if (next) index++;
                float x = 0.0f;
                if (!parseReal(ref x))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseHLineTo");
                if (rel)
                    point.x += x;
                else
                    point.x = x;
                shape.addPoint(point);
            }
            //-----------------------------------------------------------------------

            internal void parseVLineTo(bool rel, bool next) {
                if (next) index++;
                float y = 0.0f;
                if (!parseReal(ref y))
                    OGRE_EXCEPT("Exception::ERR_INVALIDPARAMS", "Expecting a Real number", "parseVLineTo");
                if (rel)
                    point.y += y;
                else
                    point.y = y;
                shape.addPoint(point);
            }
             

            #region add
            private double atan2(float uy, float ux) {
                return System.Math.Atan2(uy, ux);
            }
            private int ceil(double p) {
                return (int)System.Math.Ceiling(p);// throw new NotImplementedException();
            }

            private double abs(float p) {
                return System.Math.Abs(p);//throw new NotImplementedException();
            }

            private float sqrt(float p) {
                return Math.Sqrt(p);//throw new NotImplementedException();
            }

            private float cos(float p) {
                return Math.Cos(p);//throw new NotImplementedException();
            }

            private float sin(float p) {
                return Math.Sin(p);//throw new NotImplementedException();
            }

            private void OGRE_EXCEPT(string p, string p_2, string p_3) {
                throw new Exception(p + "_" + p_2 + "_" + p_3); //throw new NotImplementedException();
            }
            #endregion

        }
        //public:

        /**
        Parses a SVG file
        @param out MultiShape object where to store shapes from svg file
        @param fileName Filename of svg file
        @param groupName Resource group where svg file is listed
        @param segmentsNumber Number of segments for curves
        */
        //void parseSvgFile(MultiShape& out, const Ogre::String& fileName, const Ogre::String& groupName = Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME, int segmentsNumber = 8);
        public void parseSvgFile(ref MultiShape ms, string fileName) {
            parseSvgFile(ref ms, fileName, Mogre.ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, 8);
        }
        public void parseSvgFile(ref MultiShape @out, string fileName, string groupName, int segmentsNumber) {
            mNumSeg = (uint)segmentsNumber;

            //rapidxml::xml_document<> XMLDoc;    // character type defaults to char
            XmlDocument XMLDoc = new XmlDocument();
            //    DataStreamPtr stream = ResourceGroupManager::getSingleton().openResource(fileName, groupName);
            //char* svg = strdup(stream->getAsString().c_str());
            //XMLDoc.parse<0>(svg);

            //rapidxml::xml_node<>* pXmlRoot = XMLDoc.first_node("svg");
            XmlNode pXmlRoot = XMLDoc.SelectSingleNode("svg");
            //if (pXmlRoot == NULL) return;
            if (pXmlRoot == null) return;
            //rapidxml::xml_node<>* pXmlChildNode = pXmlRoot->first_node();
            XmlNode pXmlChildNode = pXmlRoot.FirstChild;
            while (pXmlChildNode != null) {
                parseChildNode(ref @out, pXmlChildNode);
                //pXmlChildNode = pXmlChildNode->next_sibling();
                pXmlChildNode = pXmlChildNode.NextSibling;
            }
        }

        //-----------------------------------------------------------------------

        //private:
        void parseChildNode(ref MultiShape @out, XmlNode pChild) {
            string name = pChild.Name; 
            if (name.Length > 3) {
                //if (stricmp(name.c_str(), "rect") == 0)
                if (stricmp(name, "rect") == 0)
                    parseRect(ref @out, pChild);
                else if (stricmp(name, "circle") == 0)
                    parseCircle(ref @out, pChild);
                else if (stricmp(name, "ellipse") == 0)
                    parseEllipse(ref @out, pChild);
                else if (stricmp(name, "polygon") == 0 || stricmp(name, "polyline") == 0)
                    parsePolygon(ref @out, pChild);
                else if (stricmp(name, "path") == 0)
                    parsePath(ref @out, pChild); // svg path is a shape
            }

            //rapidxml::xml_node<>* pSubChildNode = pChild->first_node();
            XmlNode pSubChildNode = pChild.FirstChild;
            while (pSubChildNode != null) {
                parseChildNode(ref @out, pSubChildNode);
                pSubChildNode = pSubChildNode.NextSibling;
            }
        }

        //-----------------------------------------------------------------------
        void parseRect(ref MultiShape @out, XmlNode pRectNode) {
            float width = getAttribReal(pRectNode, "width");
            float height = getAttribReal(pRectNode, "height");
            if (width <= 0.0f || height <= 0.0f) return;
            Shape s = new RectangleShape().setHeight(height).setWidth(width).realizeShape();
            //	if(pRectNode->first_attribute("id"))
            //	ss.id = pRectNode->first_attribute("id")->value();
            //Vector2 position;
            float position_x = getAttribReal(pRectNode, "x");
            float position_y = getAttribReal(pRectNode, "y");
            Vector2 position = new Vector2(position_x, position_y);
            // Our rectangle are centered, but svg rectangles are defined by their corners
            position += 0.5f * new Vector2(width, height);
            Vector2 trans = getAttribTranslate(pRectNode);
            position += trans;
            s.translate(position);

            @out.addShape(s);
        }

        //-----------------------------------------------------------------------
        void parseCircle(ref MultiShape @out, XmlNode pCircleNode) {
            float r = getAttribReal(pCircleNode, "r");
            if (r <= 0.0f) return;
            Shape s = new CircleShape().setNumSeg(mNumSeg).setRadius(r).realizeShape();
            //	if(pCircleNode->first_attribute("id"))
            //	ss.id = pCircleNode->first_attribute("id")->value();

            float position_x = getAttribReal(pCircleNode, "cx");
            float position_y = getAttribReal(pCircleNode, "cy");
            Vector2 position = new Vector2(position_x, position_y);
            Vector2 trans = getAttribTranslate(pCircleNode);
            position += trans;
            s.translate(position);
            @out.addShape(s);
        }

        //-----------------------------------------------------------------------
        void parseEllipse(ref MultiShape @out, XmlNode pEllipseNode) {
            float rx = getAttribReal(pEllipseNode, "rx");
            float ry = getAttribReal(pEllipseNode, "ry");
            if (rx <= 0.0f || ry <= 0.0f) return;
            Shape s = new EllipseShape().setNumSeg(mNumSeg).setRadiusX(rx).setRadiusY(ry).realizeShape();
            //	if(pEllipseNode->first_attribute("id"))
            //	ss.id = pEllipseNode->first_attribute("id")->value();

            float position_x = getAttribReal(pEllipseNode, "cx");
            float position_y = getAttribReal(pEllipseNode, "cy");
            Vector2 position = new Vector2(position_x, position_y);
            Vector2 trans = getAttribTranslate(pEllipseNode);
            position += trans;
            s.translate(position);
            @out.addShape(s);
        }

        //-----------------------------------------------------------------------
        void parsePolygon(ref MultiShape @out, XmlNode pPolygonNode) {
            //if (pPolygonNode->first_attribute("points"))
            if (pPolygonNode.Attributes["points"] != null) {
                //if (pPolygonNode->first_attribute("points")->value_size() < 3) return;
                if (string.IsNullOrEmpty(pPolygonNode.Attributes["points"].Value)) return;
                if (pPolygonNode.Attributes["points"].Value.Length < 3) return;
                string temp = xtrim(pPolygonNode.Attributes["points"].Value);
                std_vector<string> pts = split(temp, " ");
                if (pts.size() == 0) return;
                Shape s = new Shape();
                for (int i = 0; i < pts.size() - 1; i += 2)
                    s.addPoint(parseReal(pts[i + 0]), parseReal(pts[i + 1]));
                if (s.getPointsReference().size() == 0) return;
                s.close();
                //		if(pPolygonNode->first_attribute("id"))
                //		ss.id = pPolygonNode->first_attribute("id")->value();
                s.translate(getAttribTranslate(pPolygonNode));
                @out.addShape(s);
            }
        }


        //-----------------------------------------------------------------------
        void parsePath(ref MultiShape @out, XmlNode pPathNode) {
            //if (pPathNode->first_attribute("d"))
            if (pPathNode.Attributes["d"] != null) {
                string temp = xtrim(pPathNode.Attributes["d"].Value, " .-0123456789mMlLhHvVcCsSqQtTaAzZ");
                std_vector<string> parts = split(temp, " ");
                for (int i = 0; i < parts.size(); i++)
                    if (parts[i].Length > 1 && !(parts[i][0] == '-' || ('0' <= parts[i][0] && parts[i][0] <= '9'))) {
                        parts.insert(parts.begin() + i + 1, parts[i] + 1);
                        //parts[i].erase(1, parts[i].size());
                        parts[i] = parts[i].Remove(1);
                    }

                SvgLoaderPath sp = new SvgLoaderPath(parts, mNumSeg);
                if (!sp.isValid()) return;
                Shape ss = sp.getSvgShape();
                Vector2 line = ss.getPoint(1) - ss.getPoint(0);
                //Real deg = line.angleBetween(ss.getPoint(2) - ss.getPoint(0)).valueDegrees();
                float deg = Utils.angleBetween(line, ss.getPoint(2) - ss.getPoint(0)).ValueDegrees;
                if ((0 <= deg && deg <= 180.0f) || (-180.0f <= deg && deg <= 0))
                    ss.setOutSide(Side.SIDE_LEFT);
                else
                    ss.setOutSide(Side.SIDE_RIGHT);

                //if(pPathNode->first_attribute("id"))
                //	ss.id = pPathNode->first_attribute("id")->value();
                ss.translate(getAttribTranslate(pPathNode));
                @out.addShape(ss);
            }
        }

        //-----------------------------------------------------------------------

        float getAttribReal(XmlNode pNode, string attrib) {
            return getAttribReal(pNode, attrib, 0f);
        }
        float getAttribReal(XmlNode pNode, string attrib, float defaultValue) {
            //if (pNode->first_attribute(attrib.c_str()))
            if (pNode.Attributes[attrib] != null) {
                if (string.IsNullOrEmpty(pNode.Attributes[attrib].Value)) return defaultValue;
                //size_t len = pNode->first_attribute(attrib.c_str())->value_size();
                //    if (len == 0) return defaultValue;
                // remove units
                int len = pNode.Attributes[attrib].Value.Length;
                char[] tmp = new char[len + 1];
                //strcpy(tmp, pNode->first_attribute(attrib.c_str())->value());
                strcpy(ref tmp, pNode[attrib].Value);
                for (int i = 0; i <= len; i++)
                    if (!(tmp[i] == '.' || ('0' <= tmp[i] && tmp[i] <= '9'))) {
                        tmp[i] = (char)0;
                        break;
                    }
                // convert
                //float retVal =StringConverter::parseReal(tmp);
                float retVal = parseReal(tmp);
                //delete tmp;
                return retVal;
            }
            else
                return defaultValue;
        }
        //-----------------------------------------------------------------------
        Vector2 getAttribTranslate(XmlNode pNode) {
            //if (pNode->first_attribute("transform"))
            if (pNode.Attributes["transform"] != null) {
                string temp = (pNode.Attributes["transform"].Value);
                int begin = temp.IndexOf("translate(");//temp.find("translate(");
                //if (begin==std::string::npos)
                if (begin == -1)
                    return Vector2.ZERO;
                begin += 10;
                int end = temp.IndexOf(")");//temp.find(")", begin);
                //if (end == std::string::npos)
                if (end == -1)
                    return Vector2.ZERO;
                //std::string temp2 = temp.substr(begin, end-begin);
                string temp2 = temp.Substring(begin, end - begin);
                //std::vector<std::string> parts = split(xtrim(temp2.c_str()), std::string(" "));
                std_vector<string> parts = split(xtrim(temp2), " ");
                if (parts.size() == 2)
                    return new Vector2(parseReal(parts[0]), parseReal(parts[1]));//return Vector2(StringConverter::parseReal(parts[0]), StringConverter::parseReal(parts[1]));
                else
                    return Vector2.ZERO;
            }
            else
                return Vector2.ZERO;
        }

        //-----------------------------------------------------------------------
        std_vector<string> split(string str, string delimiters) {
            return split(str, delimiters, true);
        }
        //std_vector<std::string> split(const std::string& str, const std::string& delimiters, bool removeEmpty = true);
        std_vector<string> split(string str, string delimiters, bool removeEmpty) {
            std_vector<string> tokens = new std_vector<string>();
            //std::string::size_type delimPos = 0, tokenPos = 0, pos = 0;
            //int delimPos = 0, tokenPos = 0, pos = 0;
            //if (str.empty()) return tokens;
            if (string.IsNullOrEmpty(str)) return tokens;
            string[] strsplits = str.Split(delimiters.ToCharArray());
            foreach (var v in strsplits) {
                if (removeEmpty) {
                    if (!string.IsNullOrEmpty(v)) {
                        tokens.push_back(v);
                    }
                }
                else {
                    tokens.push_back(v);
                }
            }
            //while (true)
            //{
            //    delimPos = str.find_first_of(delimiters, pos);
            //    tokenPos = str.find_first_not_of(delimiters, pos);

            //    if (std::string::npos != delimPos)
            //    {
            //        if (std::string::npos != tokenPos)
            //        {
            //            if (tokenPos < delimPos)
            //                tokens.push_back(str.substr(pos,delimPos-pos));
            //            else
            //            {
            //                if (!removeEmpty) tokens.push_back("");
            //            }
            //        }
            //        else
            //        {
            //            if (!removeEmpty) tokens.push_back("");
            //        }
            //        pos = delimPos+1;
            //    }
            //    else
            //    {
            //        if (std::string::npos != tokenPos)
            //            tokens.push_back(str.substr(pos));
            //        else
            //        {
            //            if (!removeEmpty) tokens.push_back("");
            //        }
            //        break;
            //    }
            //}
            return tokens;
        }
        // string xtrim(const char* val, const char* achar = " .-0123456789", char rchar = ' ');
        string xtrim(string val) {
            return xtrim(val, " .-0123456789", ' ');
        }
        string xtrim(string val, string achar) {
            return xtrim(val, achar, ' ');
        }
        string xtrim(string val, string achar, char rchar) {
            if (string.IsNullOrEmpty(val))
                return "";
            int len = strlen(val);
            char[] tmp = new char[len + 1];
            strcpy(ref tmp, val);
            tmp[len] = (char)0;
            for (int i = 0; i < len; i++) {
                bool valid = false;
                //for (Ogre::uint j = 0; j < strlen(achar); j++)
                for (int j = 0; j < strlen(achar); j++) {
                    valid = (tmp[i] == achar[j]);
                    if (valid) break;
                }
                if (!valid) tmp[i] = rchar;
            }
            string temp = get_str(tmp);
            //delete tmp;
            return temp;
        }

        #region add
        private string get_str(char[] tmp) {
            string str = "";
            foreach (var v in tmp) {
                str += v.ToString();
            }
            return str;
        }
        
        private float parseReal(string p) {
            return float.Parse(p);
        }
        private float parseReal(char[] tmp) {
            string str = "";
            foreach (var v in tmp) {
                str += v.ToString();
            }
            float real = 0f;
            float.TryParse(str, out real);
            return real;
        }

        private int strlen(string val) {
            return val.Length;
        }
        int stricmp(string str1, string str2) {
            return string.Compare(str1, str2, true);
        }
        private void strcpy(ref char[] tmp, string src) {
            for (int i = 0; i < src.Length; i++) {
                tmp[i] = src[i];
            }
        }
        #endregion
    }
}




