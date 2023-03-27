// Copyright (c) 2023 Vuplex Inc. All rights reserved.
//
// Licensed under the Vuplex Commercial Software Library License, you may
// not use this file except in compliance with the License. You may obtain
// a copy of the License at
//
//     https://vuplex.com/commercial-library-license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#if UNITY_EDITOR
using UnityEditor;

namespace Vuplex.WebView.Editor {

    /// <summary>
    /// Adds a "View documentation" link to the inspector.
    /// </summary>
    [CustomEditor(typeof(DefaultPointerInputDetector))]
    public class DefaultPointerInputDetectorInspector : UnityEditor.Editor {

        public override void OnInspectorGUI() {

            DocumentationLinkDrawer.DrawDocumentationLink("https://developer.vuplex.com/webview/IPointerInputDetector");
            DrawDefaultInspector();
        }
    }
}
#endif