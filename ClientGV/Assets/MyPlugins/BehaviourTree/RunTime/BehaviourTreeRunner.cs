using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyBehaviourTree {
    public class BehaviourTreeRunner : MonoBehaviour {

        // The main behaviour tree asset
        public BehaviourTree tree;
        

        // Start is called before the first frame update
        void Start() {
            tree = tree.Clone();
            tree.Bind();
        }

        // Update is called once per frame
        void Update() {
            if (tree) {
                tree.Update();
            }
        }
        

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }
    }
}