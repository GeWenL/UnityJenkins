node{

	def gitUser = 'yunmu_git'
	// 签名 ios打包使用
    def CODE_SIGN_IDENTITY = params.证书
    def PROVISIONING_PROFILE = params.PROVISIONING_PROFILE
    def platform = params.平台
    def teamID = params.TeamID
    def bundleID = params.BundleID

	// 默认取当前工作目录，当前工作目录在固态硬盘
    def workspace = pwd()
    // 如果有指定工作目录，则设置工作目录
    if (params.工作目录 != '') {
        workspace = params.工作目录 + '/' + env.JOB_NAME
    }

    stage('更新代码'){
    	git([url: 'https://github.com/GeWenL/UnityJenkins.git', branch: params.分支, credentialsId: gitUser])
    }

    stage('制作整包') {
        def unitybin = '/Applications/Unity/Unity.app/Contents/MacOS/Unity'
        def build = 'OnAutoBuild'
 		sh "${unitybin} -batchMode -quit -projectPath ${workspace} -executeMethod EditorToolMenu.${build} -bundleId ${bundleID} -platform ${platform} -teamId ${teamID}"
    }

    stage('ios build ipa') {
        if (params.平台 == 'ios') {
            // ios平台需要调用xcode打出ipa包
            sh "xcodebuild clean -project Publish/Unity-iPhone/Unity-iPhone.xcodeproj -configuration Release -alltargets"
            sh "xcodebuild archive -project Publish/Unity-iPhone/Unity-iPhone.xcodeproj -scheme Unity-iPhone -archivePath Publish/Unity-iPhone.xcarchive"
            sh "xcodebuild -exportArchive -archivePath Publish/Unity-iPhone.xcarchive -exportPath Publish -exportOptionsPlist Build/CodeSign/ios/exportOptions.plist  CODE_SIGN_IDENTITY=${CODE_SIGN_IDENTITY} PROVISIONING_PROFILE=${PROVISIONING_PROFILE}"
            sh "mv Publish/Unity-iPhone.ipa Publish/${bundleId}-${version}.ipa"
        }
    }

    stage('上传') {

    }
}