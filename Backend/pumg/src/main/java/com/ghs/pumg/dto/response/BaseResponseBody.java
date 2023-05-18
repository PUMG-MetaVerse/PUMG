package com.ghs.pumg.dto.response;

import lombok.Getter;
import lombok.Setter;

import java.util.Map;

/**
 * 서버 요청에대한 기본 응답값(바디) 정의.
 */
@Getter
@Setter
public class BaseResponseBody {
	String message = null;
	Integer status = null;
	Map<String, Object> data;

	public BaseResponseBody() {}
	
	public BaseResponseBody(Integer statusCode){
		this.status = statusCode;
	}

	public BaseResponseBody(Integer statusCode, String message){
		this.status = statusCode;
		this.message = message;
	}
	
	public static BaseResponseBody of(Integer statusCode, String message) {
		BaseResponseBody body = new BaseResponseBody();
		body.message = message;
		body.status = statusCode;
		return body;
	}
}