using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace RotaryHeart.Lib.SerializableDictionary
{
    [Serializable]
    public class ReorderableList
    {
#if UNITY_EDITOR
        public enum ElementDisplayType
        {
            Auto,
            Expandable,
            SingleLine
        }

        #region -- Fields --

        #region -- Public --

        #region -- Delegates --

        public delegate void HeaderExpand(bool expand);

        public delegate void DrawFooterDelegate(Rect rect);

        public delegate void ActionDelegate(ReorderableList list);

        public delegate bool ActionBoolDelegate(ReorderableList list);

        public delegate void DrawHeaderDelegate(Rect rect, GUIContent label);

        public delegate void ReorderElementesDelegate(int startIndex, int newIndex);

        public delegate void AddDropdownDelegate(Rect buttonRect, ReorderableList list);

        public delegate void DragDropAppendDelegate(Object reference, ReorderableList list);

        public delegate void DrawElementDelegate(Rect rect, SerializedProperty element, GUIContent label, int index,
            bool selected, bool focused);

        public delegate Object DragDropReferenceDelegate(Object[] references, ReorderableList list);

        public delegate float GetElementHeightDelegate(SerializedProperty element, int index);

        public delegate float GetElementsHeightDelegate(ReorderableList list);

        public delegate string GetElementNameDelegate(SerializedProperty element);

        #endregion

        #region -- Events --

        public event HeaderExpand headerExpand;
        public event DrawHeaderDelegate drawHeaderCallback;
        public event DrawFooterDelegate drawFooterCallback;
        public event DrawElementDelegate drawElementCallback;
        public event DrawElementDelegate drawElementBackgroundCallback;
        public event GetElementHeightDelegate getElementHeightCallback;
        public event GetElementNameDelegate getElementNameCallback;
        public event ReorderElementesDelegate onElementsReorder;
        public event DragDropReferenceDelegate onValidateDragAndDropCallback;
        public event DragDropAppendDelegate onAppendDragDropCallback;
        public event ActionBoolDelegate onCanRemoveCallback;
        public event ActionDelegate onReorderCallback;
        public event ActionDelegate onSelectCallback;
        public event ActionDelegate onAddCallback;
        public event ActionDelegate onRemoveCallback;
        public event ActionDelegate onMouseUpCallback;
        public event ActionDelegate onChangedCallback;

        #endregion

        public bool canAdd;
        public bool canRemove;
        public bool draggable;
        public bool expandable;
        public bool multipleSelection;
        public bool isExpanded;
        public GUIContent label;
        public float headerHeight;
        public float footerHeight;
        public float slideEasing;
        public float verticalSpacing;
        public bool showDefaultBackground;
        public ElementDisplayType elementDisplayType;
        public string elementNameProperty;
        public string elementNameOverride;
        public Texture elementIcon;

        #endregion

        #region -- Private --

        #region -- Constant --

        private const float ELEMENT_EDGE_TOP = 1;
        private const float ELEMENT_EDGE_BOT = 3;
        private const float ELEMENT_HEIGHT_OFFSET = ELEMENT_EDGE_TOP + ELEMENT_EDGE_BOT;

        private static readonly int selectionHash = "ReorderableListSelection".GetHashCode();
        private static readonly int dragAndDropHash = "ReorderableListDragAndDrop".GetHashCode();

        #endregion

        internal readonly int id;

        private SerializedProperty list;
        private int controlID = -1;
        private Rect[] elementRects;
        private readonly GUIContent elementLabel;
        private ListSelection selection;
        private readonly SlideGroup slideGroup;
        private int pressIndex;

        private float elementSpacing => Mathf.Max(0, verticalSpacing - 2);

        private float pressPosition;
        private float dragPosition;
        private int dragDirection;
        private DragElement[] dragList;
        private ListSelection beforeDragSelection;

        private int dragDropControlID = -1;

        //Indicates if the pages system will be used
        private bool enablePages;

        //Indicates how much elements per page are going to be used
        private int perPageCount;

        //Current page that is being viewed
        private int currentPage;

        //How many pages are going to be used
        private int pagesCount = 1;

        //Current pages that are being displayed
        private int showPage;

        #endregion

        #endregion

        #region -- Constructors --

        public ReorderableList(SerializedProperty keys)
            : this(keys, true, true, true)
        {
        }

        public ReorderableList(SerializedProperty list, bool canAdd, bool canRemove, bool draggable)
            : this(list, canAdd, canRemove, draggable, ElementDisplayType.Auto, null, null, null)
        {
        }

        public ReorderableList(SerializedProperty list, bool canAdd, bool canRemove, bool draggable,
            ElementDisplayType elementDisplayType, string elementNameProperty, Texture elementIcon)
            : this(list, canAdd, canRemove, draggable, elementDisplayType, elementNameProperty, null, elementIcon)
        {
        }

        public ReorderableList(SerializedProperty list, bool canAdd, bool canRemove, bool draggable,
            ElementDisplayType elementDisplayType, string elementNameProperty, string elementNameOverride,
            Texture elementIcon)
        {
            if (list == null) throw new MissingListExeption();

            if (!list.isArray)
            {
                //Check if user passed in a ReorderableArray, if so, that becomes the list object
                var array = list.FindPropertyRelative("array");

                if (array == null || !array.isArray) throw new InvalidListException();

                this.list = array;
            }
            else
            {
                this.list = list;
            }

            this.canAdd = canAdd;
            this.canRemove = canRemove;
            this.draggable = draggable;
            this.elementDisplayType = elementDisplayType;
            this.elementNameProperty = elementNameProperty;
            this.elementNameOverride = elementNameOverride;
            this.elementIcon = elementIcon;

            id = GetHashCode();
            label = new GUIContent(list.displayName);

#if UNITY_5_6_OR_NEWER
            verticalSpacing = EditorGUIUtility.standardVerticalSpacing;
#else
			verticalSpacing = 2f;
#endif

            headerHeight = 18f;
            footerHeight = 13f;
            slideEasing = 0.15f;
            expandable = true;
            showDefaultBackground = true;
            multipleSelection = true;
            elementLabel = new GUIContent();

            selection = new ListSelection();
            slideGroup = new SlideGroup();
            elementRects = new Rect[0];
        }

        #endregion

        #region -- PROPERTIES --

        public SerializedProperty List
        {
            get { return list; }
            internal set { list = value; }
        }

        public bool HasList
        {
            get
            {
                try
                {
                    return list != null && list.isArray;
                }
                catch
                {
                    return false;
                }
            }
        }

        public int Length => HasList ? list.arraySize : 0;

        public int[] Selected
        {
            get { return selection.ToArray(); }
            set { selection = new ListSelection(value); }
        }

        public int Index
        {
            get { return selection.First; }
            set { selection.Select(value); }
        }

        public bool IsDragging { get; private set; }

        #endregion

        #region -- PUBLIC --

        /// <summary>
        ///     Called to draw the list
        /// </summary>
        /// <param name="rect">Position to use for the list</param>
        /// <param name="label">List name</param>
        /// <param name="enablePages">If true it will use the page system</param>
        /// <param name="perPageCount">How many elements per page will be drawn</param>
        public void DoList(Rect rect, GUIContent label, bool enablePages, int perPageCount)
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var headerRect = rect;
            headerRect.height = headerHeight;

            if (!HasList)
            {
                DrawEmpty(headerRect, label.text + " is not an Array!", GUIStyle.none, EditorStyles.helpBox);
            }
            else
            {
                this.enablePages = enablePages;
                this.perPageCount = perPageCount;

                controlID = GUIUtility.GetControlID(selectionHash, FocusType.Keyboard, rect);
                dragDropControlID = GUIUtility.GetControlID(dragAndDropHash, FocusType.Passive, rect);

                DrawHeader(headerRect, label);

                if (isExpanded)
                {
                    if (enablePages)
                    {
                        headerRect.y = headerRect.yMax + 3;
                        DrawPages(headerRect, true);
                        headerRect.y -= 7;
                    }

                    var elementBackgroundRect = rect;
                    elementBackgroundRect.yMin = headerRect.yMax;
                    elementBackgroundRect.yMax = rect.yMax - footerHeight;

                    var evt = Event.current;

                    if (list.arraySize > 0)
                    {
                        //Update element rects if not dragging. Dragging caches draw rects so no need to update
                        if (!IsDragging) UpdateElementRects(elementBackgroundRect, evt);

                        if (elementRects.Length > 0)
                        {
                            var lastElementRect = elementRects.Length;

                            if (enablePages)
                            {
                                lastElementRect = Mathf.Min(lastElementRect, perPageCount);

                                lastElementRect = Mathf.Clamp(perPageCount + currentPage * perPageCount, 0,
                                    elementRects.Length);
                            }

                            var selectableRect = elementBackgroundRect;
                            selectableRect.yMin = elementRects[0].yMin;
                            selectableRect.yMax = elementRects[lastElementRect - 1].yMax;

                            HandlePreSelection(selectableRect, evt);
                            DrawElements(elementBackgroundRect, evt);
                            HandlePostSelection(selectableRect, evt);
                        }
                    }
                    else
                    {
                        DrawEmpty(elementBackgroundRect, "Dictionary is Empty", Style.boxBackground,
                            Style.verticalLabel);
                    }

                    var footerRect = rect;
                    footerRect.yMin = elementBackgroundRect.yMax;
                    footerRect.xMin = rect.xMax - 58;

                    if (enablePages) DrawPages(new Rect(rect.x, footerRect.yMin, 0, 15));

                    DrawFooter(footerRect);
                }
            }

            EditorGUI.indentLevel = indent;
        }

        #endregion

        #region -- PRIVATE --

        private float GetElementHeight(SerializedProperty element, int index)
        {
            if (getElementHeightCallback != null)
                return getElementHeightCallback(element, index) + ELEMENT_HEIGHT_OFFSET;
            return EditorGUI.GetPropertyHeight(element, GetElementLabel(element), IsElementExpandable(element)) +
                   ELEMENT_HEIGHT_OFFSET;
        }

        private Rect GetElementDrawRect(int index, Rect desiredRect)
        {
            if (slideEasing <= 0)
                return desiredRect;
            return IsDragging
                ? slideGroup.GetRect(dragList[index].startIndex, desiredRect, slideEasing)
                : slideGroup.SetRect(index, desiredRect);
        }

        private Rect GetElementRenderRect(SerializedProperty element, Rect elementRect)
        {
            float offset = draggable ? 20 : 5;

            var rect = elementRect;
            rect.xMin += IsElementExpandable(element) ? offset + 10 : offset;
            rect.xMax -= 5;
            rect.yMin += ELEMENT_EDGE_TOP;
            rect.yMax -= ELEMENT_EDGE_BOT;

            return rect;
        }

        private void DrawHeader(Rect rect, GUIContent label)
        {
            if (showDefaultBackground && Event.current.type == EventType.Repaint)
                Style.headerBackground.Draw(rect, false, false, false, false);

            HandleDragAndDrop(rect, Event.current);

            var titleRect = rect;
            titleRect.xMin += 6f;
            titleRect.xMax -= 55f;
            titleRect.height -= 2f;
            titleRect.y++;

            label = EditorGUI.BeginProperty(titleRect, label, list);

            if (drawHeaderCallback != null)
            {
                drawHeaderCallback(titleRect, label);
            }
            else if (expandable)
            {
                titleRect.xMin += 10;

                EditorGUI.BeginChangeCheck();

                var isExpanded = EditorGUI.Foldout(titleRect, this.isExpanded, label, true);

                if (EditorGUI.EndChangeCheck()) this.isExpanded = isExpanded;
            }
            else
            {
                GUI.Label(titleRect, label, EditorStyles.label);
            }

            EditorGUI.EndProperty();

            if (elementDisplayType != ElementDisplayType.SingleLine)
            {
                var bRect1 = rect;
                bRect1.xMin = rect.xMax - 25;
                bRect1.xMax = rect.xMax - 5;

                if (GUI.Button(bRect1, Style.expandButton, Style.preButton)) ExpandElements(true);

                var bRect2 = rect;
                bRect2.xMin = bRect1.xMin - 20;
                bRect2.xMax = bRect1.xMin;

                if (GUI.Button(bRect2, Style.collapseButton, Style.preButton)) ExpandElements(false);
            }
        }

        private void ExpandElements(bool expand)
        {
            if (headerExpand != null)
            {
                headerExpand.Invoke(expand);
            }
            else
            {
                if (!isExpanded && expand) isExpanded = true;

                for (var i = 0; i < list.arraySize; i++) list.GetArrayElementAtIndex(i).isExpanded = expand;
            }
        }

        private void DrawEmpty(Rect rect, string label, GUIStyle backgroundStyle, GUIStyle labelStyle)
        {
            if (showDefaultBackground && Event.current.type == EventType.Repaint)
                backgroundStyle.Draw(rect, false, false, false, false);

            EditorGUI.LabelField(rect, label, labelStyle);
        }

        private void UpdateElementRects(Rect rect, Event evt)
        {
            //Resize array if elements changed
            int i, len = list.arraySize;

            if (len != elementRects.Length) Array.Resize(ref elementRects, len);

            if (enablePages)
            {
                if (evt.type == EventType.Repaint)
                    for (i = 0; i < len; i++)
                        elementRects[i] = Rect.zero;

                len = Mathf.Min(len, perPageCount);
            }

            if (evt.type == EventType.Repaint)
            {
                //start rect
                var elementRect = rect;
                elementRect.yMin = elementRect.yMax = rect.yMin + 2;

                var spacing = elementSpacing;

                for (i = 0; i < len; i++)
                {
                    var index = i + currentPage * perPageCount;

                    if (index >= list.arraySize)
                        break;

                    var element = list.GetArrayElementAtIndex(index);
                    //update the elementRects value for this object. Grab the last elementRect for startPosition

                    elementRect.y = elementRect.yMax;
                    elementRect.height = GetElementHeight(element, index);
                    elementRects[index] = elementRect;

                    elementRect.yMax += spacing;
                }
            }
        }

        private void DrawElements(Rect rect, Event evt)
        {
            //draw list background
            if (showDefaultBackground && evt.type == EventType.Repaint)
                Style.boxBackground.Draw(rect, false, false, false, false);

            //if not dragging, draw elements as usual
            if (!IsDragging)
            {
                int i, len = list.arraySize;

                if (enablePages) len = Mathf.Min(len, perPageCount);

                for (i = 0; i < len; i++)
                {
                    var index = i + currentPage * perPageCount;

                    if (index >= list.arraySize)
                        break;

                    var selected = selection.Contains(index);

                    DrawElement(list.GetArrayElementAtIndex(index), GetElementDrawRect(index, elementRects[index]),
                        index, selected, selected && GUIUtility.keyboardControl == controlID);
                }
            }
            else if (evt.type == EventType.Repaint)
            {
                //draw dragging elements only when repainting
                int i, s, len = dragList.Length;
                var sLen = selection.Length;

                //first, find the rects of the selected elements, we need to use them for overlap queries
                for (i = 0; i < sLen; i++)
                {
                    var element = dragList[i];

                    //update the element desiredRect if selected. Selected elements appear first in the dragList, so other elements later in iteration will have rects to compare
                    element.desiredRect.y = dragPosition - element.dragOffset;
                    dragList[i] = element;
                }

                //draw elements, start from the bottom of the list as first elements are the ones selected, so should be drawn last
                i = len;

                while (--i > -1)
                {
                    var element = dragList[i];

                    if (element.property == null)
                        continue;

                    //draw dragging elements last as the loop is backwards
                    if (element.selected)
                    {
                        DrawElement(element.property, element.desiredRect, element.startIndex, true, true);
                        continue;
                    }

                    //loop over selection and see what overlaps
                    //if dragging down we start from the bottom of the selection
                    //otherwise we start from the top. This helps to cover multiple selected objects
                    var elementRect = element.rect;

                    if (elementRect.Equals(Rect.zero))
                        continue;

                    var elementIndex = element.startIndex;

                    var start = dragDirection > 0 ? sLen - 1 : 0;
                    var end = dragDirection > 0 ? -1 : sLen;

                    for (s = start; s != end; s -= dragDirection)
                    {
                        var selected = dragList[s];

                        if (selected.Overlaps(elementRect, elementIndex, dragDirection))
                        {
                            elementRect.y -= selected.rect.height * dragDirection;
                            elementIndex += dragDirection;
                        }
                    }

                    //draw the element with the new rect
                    DrawElement(element.property, GetElementDrawRect(i, elementRect), element.startIndex, false, false);

                    //reassign the element back into the dragList
                    element.desiredRect = elementRect;
                    dragList[i] = element;
                }
            }
        }

        private void DrawElement(SerializedProperty element, Rect rect, int index, bool selected, bool focused)
        {
            var evt = Event.current;

            if (drawElementBackgroundCallback != null)
                drawElementBackgroundCallback(rect, element, null, index, selected, focused);
            else if (evt.type == EventType.Repaint)
                if (selected)
                {
                    var color = GUI.color;
                    GUI.color = Style.selectedColor;
                    Style.selectedStyle.Draw(rect, false, false, false, false);
                    GUI.color = color;
                }

            if (evt.type == EventType.Repaint && draggable)
                Style.draggingHandle.Draw(new Rect(rect.x + 5, rect.y + 6, 10, rect.height - (rect.height - 6)), false,
                    false, false, false);

            var label = GetElementLabel(element);

            var renderRect = GetElementRenderRect(element, rect);

            if (drawElementCallback != null)
                drawElementCallback(renderRect, element, label, index, selected, focused);
            else
                EditorGUI.PropertyField(renderRect, element, label, true);
        }

        private GUIContent GetElementLabel(SerializedProperty element)
        {
            string name;

            if (getElementNameCallback != null)
                name = getElementNameCallback(element);
            else
                name = GetElementName(element, elementNameProperty, elementNameOverride);

            elementLabel.text = !string.IsNullOrEmpty(name) ? name : element.displayName;
            elementLabel.tooltip = element.tooltip;
            elementLabel.image = elementIcon;

            return elementLabel;
        }

        private static string GetElementName(SerializedProperty element, string nameProperty, string nameOverride)
        {
            if (!string.IsNullOrEmpty(nameOverride))
            {
                var path = element.propertyPath;

                if (path.EndsWith("]"))
                {
                    var startIndex = path.LastIndexOf('[') + 1;

                    return string.Concat(nameOverride, " ", path.Substring(startIndex, path.Length - startIndex - 1));
                }

                return nameOverride;
            }

            if (string.IsNullOrEmpty(nameProperty))
                return null;
            if (element.propertyType == SerializedPropertyType.ObjectReference && nameProperty == "name")
                return element.objectReferenceValue ? element.objectReferenceValue.name : null;

            var prop = element.FindPropertyRelative(nameProperty);

            if (prop != null)
            {
                switch (prop.propertyType)
                {
                    case SerializedPropertyType.ObjectReference:

                        return prop.objectReferenceValue ? prop.objectReferenceValue.name : null;

                    case SerializedPropertyType.Enum:

                        return prop.enumDisplayNames[prop.enumValueIndex];

                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.Character:

                        return prop.intValue.ToString();

                    case SerializedPropertyType.LayerMask:

                        return GetLayerMaskName(prop.intValue);

                    case SerializedPropertyType.String:

                        return prop.stringValue;

                    case SerializedPropertyType.Float:

                        return prop.floatValue.ToString();
                }

                return prop.displayName;
            }

            return null;
        }

        private static string GetLayerMaskName(int mask)
        {
            if (mask == 0)
                return "Nothing";
            if (mask < 0) return "Everything";

            var name = string.Empty;
            var n = 0;

            for (var i = 0; i < 32; i++)
                if (((1 << i) & mask) != 0)
                {
                    if (n == 4) return "Mixed ...";

                    name += (n > 0 ? ", " : string.Empty) + LayerMask.LayerToName(i);
                    n++;
                }

            return name;
        }

        private void DrawPages(Rect rect, bool isHeader = false)
        {
            var labelRect = new Rect(rect.x, rect.y - 3, 40, 15);

            if (Event.current.type == EventType.Repaint)
                Style.footerBackground.Draw(new Rect(labelRect.xMin - 1, rect.y, isHeader ? rect.width + 1 : 185, 15),
                    false, false, false, false);

            pagesCount = Mathf.CeilToInt(list.arraySize / (float) perPageCount);

            GUI.Label(labelRect, currentPage + 1 + "/" + pagesCount, Style.preButton);

            var moveBack = new Rect(labelRect.xMax, labelRect.yMin, 20, 15);
            var page1Rect = new Rect(moveBack.xMax, labelRect.yMin, 25, 15);
            var page2Rect = new Rect(page1Rect.xMax, labelRect.yMin, 25, 15);
            var page3Rect = new Rect(page2Rect.xMax, labelRect.yMin, 25, 15);
            var page4Rect = new Rect(page3Rect.xMax, labelRect.yMin, 25, 15);
            var moveNext = new Rect(page4Rect.xMax, labelRect.yMin, 20, 15);

            if (GUI.Button(moveBack, char.ConvertFromUtf32(0x2190), Style.preButton))
            {
                showPage -= 4;
                showPage = Mathf.Clamp(showPage, 0, pagesCount);
            }

            GUI.enabled = currentPage != showPage;
            if (pagesCount > 0)
                if (GUI.Button(page1Rect, (showPage + 1).ToString(), Style.preButton))
                {
                    currentPage = showPage;
                    selection.Clear();
                }

            GUI.enabled = true;

            GUI.enabled = currentPage != showPage + 1;
            var drawButton = false;
            if (showPage > 3)
            {
                if (pagesCount > showPage + 1) drawButton = true;
            }
            else if (pagesCount > 1)
            {
                drawButton = true;
            }

            if (drawButton)
                if (GUI.Button(page2Rect, (showPage + 2).ToString(), Style.preButton))
                {
                    currentPage = showPage + 1;
                    selection.Clear();
                }

            GUI.enabled = true;

            GUI.enabled = currentPage != showPage + 2;
            drawButton = false;
            if (showPage > 3)
            {
                if (pagesCount > showPage + 2) drawButton = true;
            }
            else if (pagesCount > 2)
            {
                drawButton = true;
            }

            if (drawButton)
                if (GUI.Button(page3Rect, (showPage + 3).ToString(), Style.preButton))
                {
                    currentPage = showPage + 2;
                    selection.Clear();
                }

            GUI.enabled = true;

            GUI.enabled = currentPage != showPage + 3;
            drawButton = false;
            if (showPage > 4)
            {
                if (pagesCount > showPage + 3) drawButton = true;
            }
            else if (pagesCount > 3)
            {
                drawButton = true;
            }

            if (drawButton)
                if (GUI.Button(page4Rect, (showPage + 4).ToString(), Style.preButton))
                {
                    currentPage = showPage + 3;
                    selection.Clear();
                }

            GUI.enabled = true;

            if (GUI.Button(moveNext, char.ConvertFromUtf32(0x2192), Style.preButton))
                if (pagesCount > 4)
                {
                    showPage += 4;
                    showPage = Mathf.Clamp(showPage, 0, pagesCount - 4);
                }
        }

        private void DrawFooter(Rect rect)
        {
            if (drawFooterCallback != null)
            {
                drawFooterCallback(rect);
                return;
            }

            if (Event.current.type == EventType.Repaint) Style.footerBackground.Draw(rect, false, false, false, false);

            var addRect = new Rect(rect.xMin + 4f, rect.y - 3f, 25f, 13f);
            var subRect = new Rect(rect.xMax - 29f, rect.y - 3f, 25f, 13f);

            EditorGUI.BeginDisabledGroup(!canAdd);

            if (GUI.Button(addRect, Style.iconToolbarPlus, Style.preButton))
            {
                onAddCallback(this);
                currentPage = pagesCount - 1;
            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!CanSelect(selection) || !canRemove ||
                                         onCanRemoveCallback != null && !onCanRemoveCallback(this));

            if (GUI.Button(subRect, Style.iconToolbarMinus, Style.preButton))
            {
                selection.Sort();

                onRemoveCallback(this);

                selection.Clear();
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DispatchChange()
        {
            if (onChangedCallback != null) onChangedCallback(this);
        }

        private void HandleDragAndDrop(Rect rect, Event evt)
        {
            switch (evt.GetTypeForControl(dragDropControlID))
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (GUI.enabled && rect.Contains(evt.mousePosition))
                    {
                        var objectReferences = DragAndDrop.objectReferences;
                        var references = new Object[1];

                        var acceptDrag = false;

                        foreach (var object1 in objectReferences)
                        {
                            references[0] = object1;
                            var object2 = ValidateObjectDragAndDrop(references);

                            if (object2 != null)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                                if (evt.type == EventType.DragPerform)
                                {
                                    if (onAppendDragDropCallback != null)
                                        onAppendDragDropCallback(object2, this);
                                    else
                                        AppendDragAndDropValue(object2);

                                    acceptDrag = true;
                                    DragAndDrop.activeControlID = 0;
                                }
                                else
                                {
                                    DragAndDrop.activeControlID = dragDropControlID;
                                }
                            }
                        }

                        if (acceptDrag)
                        {
                            GUI.changed = true;
                            DragAndDrop.AcceptDrag();
                        }
                    }

                    break;

                case EventType.DragExited:
                    if (GUI.enabled) HandleUtility.Repaint();
                    break;
            }
        }

        private Object ValidateObjectDragAndDrop(Object[] references)
        {
            if (onValidateDragAndDropCallback != null) return onValidateDragAndDropCallback(references, this);

            return Internals.ValidateObjectDragAndDrop(references, list);
        }

        private void AppendDragAndDropValue(Object obj)
        {
            Internals.AppendDragAndDropValue(obj, list);

            DispatchChange();
        }

        private void HandlePreSelection(Rect rect, Event evt)
        {
            if (evt.type == EventType.MouseDrag && draggable && GUIUtility.hotControl == controlID)
            {
                if (selection.Length > 0 && UpdateDragPosition(evt.mousePosition, rect, dragList))
                {
                    GUIUtility.keyboardControl = controlID;
                    IsDragging = true;
                }

                evt.Use();
            }

            /* TODO This is buggy. The reason for this is to allow selection and dragging of an element using the header, or top row (if any)
			 * The main issue here is determining whether the element has an "expandable" drop down arrow, which if it does, will capture the mouse event *without* the code below
			 * Because of property drawers and certain property types, it's impossible to know this automatically (without dirty reflection)
			 * So if the below code is active and we determine that the property is expandable but isn't actually. Then we'll accidently capture the mouse focus and prevent anything else from receiving it :(
			 * So for now, in order to drag or select a row, the user must select empty space on the row. Not a huge deal, and doesn't break functionality.
			 * What needs to happen is the drag event needs to occur independent of the event type. But that's messy too, as some controls have horizontal drag sliders :(
			if (evt.type == EventType.MouseDown) {

				//check if we contain the mouse press
				//we also need to check what has current focus. If nothing we can assume control
				//if there's something, check if the header has been pressed if the element is expandable
				//if we did press the header, then override the control

				if (rect.Contains(evt.mousePosition) && IsSelectionButton(evt)) {

					int index = GetSelectionIndex(evt.mousePosition);

					if (CanSelect(index)) {

						SerializedProperty element = list.GetArrayElementAtIndex(index);

						if (IsElementExpandable(element)) {

							Rect elementHeaderRect = GetElementHeaderRect(element, elementRects[index]);
							Rect elementRenderRect = GetElementRenderRect(element, elementRects[index]);

							Rect elementExpandRect = elementHeaderRect;
							elementExpandRect.xMin = elementRenderRect.xMin - 10;
							elementExpandRect.xMax = elementRenderRect.xMin;

							if (elementHeaderRect.Contains(evt.mousePosition) && !elementExpandRect.Contains(evt.mousePosition)) {

								DoSelection(index, true, evt);
								HandleUtility.Repaint();
							}
						}
					}
				}
			}
			*/
        }

        private void HandlePostSelection(Rect rect, Event evt)
        {
            switch (evt.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:

                    if (rect.Contains(evt.mousePosition) && IsSelectionButton(evt))
                    {
                        var index = GetSelectionIndex(evt.mousePosition);

                        if (CanSelect(index))
                            DoSelection(index,
                                GUIUtility.keyboardControl == 0 || GUIUtility.keyboardControl == controlID ||
                                evt.button == 2, evt);
                        else
                            selection.Clear();

                        HandleUtility.Repaint();
                    }

                    break;

                case EventType.MouseUp:

                    if (!draggable)
                    {
                        //select the single object if no selection modifier is being performed
                        selection.SelectWhenNoAction(pressIndex, evt);

                        if (onMouseUpCallback != null && IsPositionWithinElement(evt.mousePosition, selection.Last))
                            onMouseUpCallback(this);
                    }
                    else if (GUIUtility.hotControl == controlID)
                    {
                        evt.Use();

                        if (IsDragging)
                        {
                            IsDragging = false;

                            //move elements in list sort the drag list
                            ReorderDraggedElements(dragList);

                            //apply changes
                            list.serializedObject.ApplyModifiedProperties();
                            list.serializedObject.Update();

                            if (onReorderCallback != null) onReorderCallback(this);

                            DispatchChange();
                        }
                        else
                        {
                            //if we didn't drag, then select the original pressed object
                            selection.SelectWhenNoAction(pressIndex, evt);

                            if (onMouseUpCallback != null) onMouseUpCallback(this);
                        }

                        GUIUtility.hotControl = 0;
                    }

                    HandleUtility.Repaint();

                    break;

                case EventType.KeyDown:

                    if (GUIUtility.keyboardControl == controlID)
                    {
                        if (evt.keyCode == KeyCode.DownArrow && !IsDragging)
                        {
                            selection.Select(Mathf.Min(selection.Last + 1, list.arraySize - 1));
                            evt.Use();
                        }
                        else if (evt.keyCode == KeyCode.UpArrow && !IsDragging)
                        {
                            selection.Select(Mathf.Max(selection.Last - 1, 0));
                            evt.Use();
                        }
                        else if (evt.keyCode == KeyCode.Escape && GUIUtility.hotControl == controlID)
                        {
                            GUIUtility.hotControl = 0;

                            if (IsDragging)
                            {
                                IsDragging = false;
                                selection = beforeDragSelection;
                            }

                            evt.Use();
                        }
                    }

                    break;
            }
        }

        private bool IsSelectionButton(Event evt)
        {
            return evt.button == 0 || evt.button == 2;
        }

        private void DoSelection(int index, bool setKeyboardControl, Event evt)
        {
            //append selections based on action, this may be a additive (ctrl) or range (shift) selection
            if (multipleSelection)
                selection.AppendWithAction(pressIndex = index, evt);
            else
                selection.Select(pressIndex = index);

            if (onSelectCallback != null) onSelectCallback(this);

            if (draggable)
            {
                IsDragging = false;
                dragPosition = pressPosition = evt.mousePosition.y;
                dragList = GetDragList(dragPosition);

                beforeDragSelection = selection.Clone();

                GUIUtility.hotControl = controlID;
            }

            if (setKeyboardControl) GUIUtility.keyboardControl = controlID;

            evt.Use();
        }

        private DragElement[] GetDragList(float dragPosition)
        {
            int i, len = list.arraySize;

            if (dragList == null)
                dragList = new DragElement[len];
            else if (dragList.Length != len) Array.Resize(ref dragList, len);

            if (enablePages)
            {
                for (i = 0; i < len; i++)
                {
                    dragList[i].property = null;
                    dragList[i].dragOffset = elementRects[i].y;
                    dragList[i].rect = elementRects[i];
                    dragList[i].desiredRect = elementRects[i];
                    dragList[i].selected = false;
                    dragList[i].startIndex = i;
                }

                len = Mathf.Min(len, perPageCount);
            }

            for (i = 0; i < len; i++)
            {
                var index = i + currentPage * perPageCount;

                if (index >= list.arraySize)
                    break;

                var property = list.GetArrayElementAtIndex(index);
                var elementRect = elementRects[index];

                var dragElement = new DragElement
                {
                    property = property,
                    dragOffset = dragPosition - elementRect.y,
                    rect = elementRect,
                    desiredRect = elementRect,
                    selected = selection.Contains(index),
                    startIndex = index
                };

                dragList[index] = dragElement;
            }

            //finally, sort the dragList by selection, selected objects appear first in the list
            //selection order is preserved as well
            Array.Sort(dragList, (a, b) =>
            {
                if (b.selected)
                    return a.selected ? a.startIndex.CompareTo(b.startIndex) : 1;
                if (a.selected) return b.selected ? b.startIndex.CompareTo(a.startIndex) : -1;

                return a.startIndex.CompareTo(b.startIndex);
            });

            return dragList;
        }

        private bool UpdateDragPosition(Vector2 position, Rect bounds, DragElement[] dragList)
        {
            //find new drag position
            var startIndex = 0;
            var endIndex = selection.Length - 1;

            var minOffset = dragList[startIndex].dragOffset;
            var maxOffset = dragList[endIndex].rect.height - dragList[endIndex].dragOffset;

            dragPosition = Mathf.Clamp(position.y, bounds.yMin + minOffset, bounds.yMax - maxOffset);

            if (Mathf.Abs(dragPosition - pressPosition) > 1)
            {
                dragDirection = (int) Mathf.Sign(dragPosition - pressPosition);
                return true;
            }

            return false;
        }

        private void ReorderDraggedElements(DragElement[] dragList)
        {
            //Save the current expanded states on all elements. I don't see any other way to do this
            //MoveArrayElement does not move the foldout states, so... fun.
            for (var i = 0; i < dragList.Length; i++)
            {
                if (dragList[i].property == null)
                    continue;

                dragList[i].RecordState();
            }

            //Sort list based on positions
            Array.Sort(dragList, (a, b) => a.desiredRect.center.y.CompareTo(b.desiredRect.center.y));

            selection.Sort((a, b) =>
            {
                var d1 = GetDragIndexFromSelection(a);
                var d2 = GetDragIndexFromSelection(b);

                return dragDirection > 0 ? d1.CompareTo(d2) : d2.CompareTo(d1);
            });

            //Swap the selected elements in the List
            var s = selection.Length;

            while (--s > -1)
            {
                var newIndex = GetDragIndexFromSelection(selection[s]);

                selection[s] = newIndex;
                var correctIndex = newIndex;

                if (enablePages)
                {
                    var tempIndex = -1;

                    for (var i = 0; i < dragList.Length; i++)
                    {
                        if (!dragList[i].rect.Equals(Rect.zero))
                            tempIndex++;

                        correctIndex = i;

                        if (tempIndex == newIndex)
                            break;
                    }

                    newIndex = newIndex + perPageCount * currentPage;
                }

                list.MoveArrayElement(dragList[correctIndex].startIndex, newIndex);

                if (onElementsReorder != null) onElementsReorder.Invoke(dragList[correctIndex].startIndex, newIndex);
            }

            //Restore expanded states on items
            for (var i = 0; i < dragList.Length; i++)
            {
                if (dragList[i].property == null)
                    continue;

                dragList[i].RestoreState(list.GetArrayElementAtIndex(i));
            }
        }

        private int GetDragIndexFromSelection(int index)
        {
            if (enablePages)
            {
                var returnIndex = -1;

                for (var i = 0; i < dragList.Length; i++)
                {
                    if (!dragList[i].rect.Equals(Rect.zero))
                        returnIndex++;

                    if (dragList[i].startIndex == index) return returnIndex;
                }

                return index;
            }

            return Array.FindIndex(dragList, t => t.startIndex == index && !t.rect.Equals(Rect.zero));
        }

        private int GetSelectionIndex(Vector2 position)
        {
            int i, len = elementRects.Length;

            if (enablePages) len = Mathf.Min(len, perPageCount);

            for (i = 0; i < len; i++)
            {
                var index = i + currentPage * perPageCount;

                if (index >= elementRects.Length)
                    break;

                var rect = elementRects[index];

                if (rect.Contains(position) || index == 0 && position.y <= rect.yMin ||
                    index == len - 1 && position.y >= rect.yMax) return index;
            }

            return -1;
        }

        private bool CanSelect(ListSelection selection)
        {
            return selection.Length > 0 ? selection.All(s => CanSelect(s)) : false;
        }

        private bool CanSelect(int index)
        {
            return index >= 0 && index < list.arraySize;
        }

        private bool IsPositionWithinElement(Vector2 position, int index)
        {
            return CanSelect(index) ? elementRects[index].Contains(position) : false;
        }

        private bool IsElementExpandable(SerializedProperty element)
        {
            switch (elementDisplayType)
            {
                case ElementDisplayType.Auto:
                    return element.hasVisibleChildren && IsTypeExpandable(element.propertyType);

                case ElementDisplayType.Expandable: return true;
                case ElementDisplayType.SingleLine: return false;
            }

            return false;
        }

        private bool IsTypeExpandable(SerializedPropertyType type)
        {
            switch (type)
            {
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.ArraySize:
                    return true;

                default:
                    return false;
            }
        }

        #endregion

        #region -- LIST STYLE --

        private static class Style
        {
            public static readonly GUIContent iconToolbarPlus;
            public static GUIContent iconToolbarPlusMore;
            public static readonly GUIContent iconToolbarMinus;
            public static readonly GUIStyle draggingHandle;
            public static readonly GUIStyle headerBackground;
            public static readonly GUIStyle footerBackground;
            public static readonly GUIStyle boxBackground;
            public static readonly GUIStyle preButton;
            public static GUIStyle elementBackground;
            public static readonly GUIStyle verticalLabel;
            public static readonly GUIStyle selectedStyle;
            public static readonly GUIContent expandButton;
            public static readonly GUIContent collapseButton;
            public static readonly Color selectedColor;


            static Style()
            {
                iconToolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus", "Add to list");
                iconToolbarPlusMore = EditorGUIUtility.IconContent("Toolbar Plus More", "Choose to add to list");
                iconToolbarMinus = EditorGUIUtility.IconContent("Toolbar Minus", "Remove selection from list");
                draggingHandle = new GUIStyle("RL DragHandle");
                headerBackground = new GUIStyle("RL Header");
                footerBackground = new GUIStyle("RL Footer");
                elementBackground = new GUIStyle("RL Element")
                {
                    border = new RectOffset(2, 3, 2, 3)
                };
                verticalLabel = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleLeft,
                    contentOffset = new Vector2(10, -3)
                };
                boxBackground = new GUIStyle("RL Background")
                {
                    border = new RectOffset(6, 3, 3, 6)
                };
                preButton = new GUIStyle("RL FooterButton");
                preButton.normal.textColor = verticalLabel.normal.textColor;
                selectedStyle = new GUIStyle
                {
                    normal = new GUIStyleState {background = Texture2D.whiteTexture}
                };
                expandButton = EditorGUIUtility.IconContent("winbtn_win_max");
                collapseButton = EditorGUIUtility.IconContent("winbtn_win_min");
                selectedColor = GUI.skin.settings.selectionColor;
            }
        }

        #endregion

        #region -- DRAG ELEMENT --

        private struct DragElement
        {
            internal SerializedProperty property;
            internal int startIndex;
            internal float dragOffset;
            internal bool selected;
            internal Rect rect;
            internal Rect desiredRect;

            private bool isExpanded;
            private Dictionary<int, bool> states;

            internal bool Overlaps(Rect value, int index, int direction)
            {
                if (direction < 0 && index < startIndex)
                    return desiredRect.yMin < value.center.y;
                if (direction > 0 && index > startIndex) return desiredRect.yMax > value.center.y;

                return false;
            }

            internal void RecordState()
            {
                states = new Dictionary<int, bool>();
                isExpanded = property.isExpanded;

                Iterate(this, property, (e, p, index) => { e.states[index] = p.isExpanded; });
            }

            internal void RestoreState(SerializedProperty property)
            {
                property.isExpanded = isExpanded;

                Iterate(this, property, (e, p, index) => { p.isExpanded = e.states[index]; });
            }

            private static void Iterate(DragElement element, SerializedProperty property,
                Action<DragElement, SerializedProperty, int> action)
            {
                var copy = property.Copy();
                var end = copy.GetEndProperty();

                var index = 0;

                while (copy.NextVisible(true) && !SerializedProperty.EqualContents(copy, end))
                    if (copy.hasVisibleChildren)
                    {
                        action(element, copy, index);
                        index++;
                    }
            }
        }

        #endregion

        #region -- SLIDE GROUP --

        private class SlideGroup
        {
            private readonly Dictionary<int, Rect> animIDs;

            public SlideGroup()
            {
                animIDs = new Dictionary<int, Rect>();
            }

            public Rect GetRect(int id, Rect r, float easing)
            {
                if (Event.current.type != EventType.Repaint) return r;

                if (!animIDs.ContainsKey(id))
                {
                    animIDs.Add(id, r);
                    return r;
                }

                var rect = animIDs[id];

                if (rect.y != r.y)
                {
                    var delta = r.y - rect.y;
                    var absDelta = Mathf.Abs(delta);

                    //if the distance between current rect and target is too large, then move the element towards the target rect so it reaches the destination faster
                    if (absDelta > rect.height * 2)
                        r.y = delta > 0 ? r.y - rect.height : r.y + rect.height;
                    else if (absDelta > 0.5) r.y = Mathf.Lerp(rect.y, r.y, easing);

                    animIDs[id] = r;
                    HandleUtility.Repaint();
                }

                return r;
            }

            public Rect SetRect(int id, Rect rect)
            {
                if (animIDs.ContainsKey(id))
                    animIDs[id] = rect;
                else
                    animIDs.Add(id, rect);

                return rect;
            }
        }

        #endregion

        #region -- SELECTION --

        private class ListSelection : IEnumerable<int>
        {
            internal int? firstSelected;
            private readonly List<int> indexes;

            public ListSelection()
            {
                indexes = new List<int>();
            }

            public ListSelection(int[] indexes)
            {
                this.indexes = new List<int>(indexes);
            }

            public int First => indexes.Count > 0 ? indexes[0] : -1;

            public int Last => indexes.Count > 0 ? indexes[indexes.Count - 1] : -1;

            public int Length => indexes.Count;

            public int this[int index]
            {
                get { return indexes[index]; }
                set
                {
                    var oldIndex = indexes[index];

                    indexes[index] = value;

                    if (oldIndex == firstSelected) firstSelected = value;
                }
            }

            public IEnumerator<int> GetEnumerator()
            {
                return ((IEnumerable<int>) indexes).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<int>) indexes).GetEnumerator();
            }

            public bool Contains(int index)
            {
                return indexes.Contains(index);
            }

            public void Clear()
            {
                indexes.Clear();
                firstSelected = null;
            }

            public void SelectWhenNoAction(int index, Event evt)
            {
                if (!EditorGUI.actionKey && !evt.shift) Select(index);
            }

            public void Select(int index)
            {
                indexes.Clear();
                indexes.Add(index);

                firstSelected = index;
            }

            public void Remove(int index)
            {
                if (indexes.Contains(index)) indexes.Remove(index);
            }

            public void AppendWithAction(int index, Event evt)
            {
                if (EditorGUI.actionKey)
                {
                    if (Contains(index))
                    {
                        Remove(index);
                    }
                    else
                    {
                        Append(index);
                        firstSelected = index;
                    }
                }
                else if (evt.shift && indexes.Count > 0 && firstSelected.HasValue)
                {
                    indexes.Clear();

                    AppendRange(firstSelected.Value, index);
                }
                else if (!Contains(index))
                {
                    Select(index);
                }
            }

            public void Sort()
            {
                if (indexes.Count > 0) indexes.Sort();
            }

            public void Sort(Comparison<int> comparison)
            {
                if (indexes.Count > 0) indexes.Sort(comparison);
            }

            public int[] ToArray()
            {
                return indexes.ToArray();
            }

            public ListSelection Clone()
            {
                var clone = new ListSelection(ToArray());
                clone.firstSelected = firstSelected;

                return clone;
            }

            internal bool CanRevert(SerializedProperty list)
            {
                if (list.serializedObject.targetObjects.Length == 1)
                    for (var i = 0; i < Length; i++)
                        if (list.GetArrayElementAtIndex(this[i]).isInstantiatedPrefab)
                            return true;

                return false;
            }

            internal void RevertValues(object userData)
            {
                var list = userData as SerializedProperty;

                for (var i = 0; i < Length; i++)
                {
                    var property = list.GetArrayElementAtIndex(this[i]);

                    if (property.isInstantiatedPrefab) property.prefabOverride = false;
                }

                list.serializedObject.ApplyModifiedProperties();
                list.serializedObject.Update();

                HandleUtility.Repaint();
            }

            private void Append(int index)
            {
                if (index >= 0 && !indexes.Contains(index)) indexes.Add(index);
            }

            private void AppendRange(int from, int to)
            {
                var dir = (int) Mathf.Sign(to - from);

                if (dir != 0)
                    for (var i = from; i != to; i += dir)
                        Append(i);

                Append(to);
            }
        }

        #endregion

        #region -- EXCEPTIONS --

        private class InvalidListException : InvalidOperationException
        {
            public InvalidListException() : base("ReorderableList serializedProperty must be an array")
            {
            }
        }

        private class MissingListExeption : ArgumentNullException
        {
            public MissingListExeption() : base("ReorderableList serializedProperty is null")
            {
            }
        }

        #endregion

        #region -- INTERNAL --

        private static class Internals
        {
            private static readonly MethodInfo dragDropValidation;
            private static object[] dragDropValidationParams;
            private static readonly MethodInfo appendDragDrop;
            private static object[] appendDragDropParams;

            static Internals()
            {
                dragDropValidation = Type.GetType("UnityEditor.EditorGUI, UnityEditor")
                    .GetMethod("ValidateObjectFieldAssignment", BindingFlags.NonPublic | BindingFlags.Static);
                appendDragDrop = typeof(SerializedProperty).GetMethod("AppendFoldoutPPtrValue",
                    BindingFlags.NonPublic | BindingFlags.Instance);
            }

            internal static Object ValidateObjectDragAndDrop(Object[] references, SerializedProperty property)
            {
#if UNITY_2017_1_OR_NEWER
                dragDropValidationParams = GetParams(ref dragDropValidationParams, 4);
                dragDropValidationParams[0] = references;
                dragDropValidationParams[1] = null;
                dragDropValidationParams[2] = property;
                dragDropValidationParams[3] = 0;
#else
                dragDropValidationParams = GetParams(ref dragDropValidationParams, 3);
                dragDropValidationParams[0] = references;
                dragDropValidationParams[1] = null;
                dragDropValidationParams[2] = property;
#endif
                return dragDropValidation.Invoke(null, dragDropValidationParams) as Object;
            }

            internal static void AppendDragAndDropValue(Object obj, SerializedProperty list)
            {
                appendDragDropParams = GetParams(ref appendDragDropParams, 1);

                Debug.Log(appendDragDropParams.Length);

                appendDragDropParams[0] = obj;
                appendDragDrop.Invoke(list, appendDragDropParams);
            }

            private static object[] GetParams(ref object[] parameters, int count)
            {
                if (parameters == null) parameters = new object[count];

                return parameters;
            }
        }

        #endregion

#endif
    }
}