parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
cd "$parent_path"

unityVersion=`cat ./ProjectSettings/ProjectVersion.txt | grep m_EditorVersion: |  rev | cut -d: -f1 | rev | sed 's/^[[:blank:]]*//;s/[[:blank:]]*$//'`

RESULTS_FILE=$PWD/build/logs/TestResult.xml

~/Unity/Hub/Editor/$unityVersion/Editor/Unity \
    -batchmode \
    -logfile \
    -nographics \
    -runEditorTests \
    -editorTestsResultFile $RESULTS_FILE


if grep -q "failed=\"0\"" "$RESULTS_FILE"; then
    echo "tests passed";
    exit 0;
else
    echo "tests failed";
fi
