export default {
    getUrlParas: () => {
        let url = window.location.href;
        let urlSplit = url.split('?');
        if (urlSplit.length != 2)
            return null;
        let paramStr=urlSplit[1];
        let paramsSplit= paramStr.split('&')
        let params={};
        for(var i=0;i<paramsSplit.length;i++){
            let keyValue=paramsSplit[i].split('=');
            let key=keyValue[0];
            let value=keyValue[1];
            params[key]=value;
        }
        return params;
    }
}