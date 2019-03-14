parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
cd "$parent_path"

unityVersion=`cat ./ProjectSettings/ProjectVersion.txt | grep m_EditorVersion: |  rev | cut -d: -f1 | rev | sed 's/^[[:blank:]]*//;s/[[:blank:]]*$//'`

~/Unity/Hub/Editor/$unityVersion/Editor/Unity \
    -quit \
    -batchmode \
    -logfile \
    -nographics \
    "$@" \
    -executeMethod  MyEditorBuild.PerformBuild


