import axios from 'axios'
export default {
    domain: 'http://www.hnyouyang.com:81',
    // domain: 'http://localhost:5000',
    getUserIdByCode: function (code: string,callback:Function): void {
        let url = this.domain + '/api/GetUserIdByCode';
        let data = { code };
        axios.interceptors.request.use(config => {
            //指定客户端能够接收的内容类型
            config.headers['Access-Control-Allow-Origin'] = "*"
            return config;
            }, error => Promise.error(error)
            )
        axios.post(url, data).then(function(res){
            console.log(res);
            callback(res.data);
        }).catch(function(err){
            console.log(err);
        });
    },
    getAA(){
        let data={
            code:'123'
        }
        axios.post(this.domain+"/api/aaa",data)
        .then(ret=>{
            console.log(ret);
        })
    }
}