package com.ghs.pumg.exception;

import com.ghs.pumg.dto.response.BaseResponseBody;
import lombok.extern.slf4j.Slf4j;
import org.springframework.core.io.ClassPathResource;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.util.StreamUtils;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.servlet.NoHandlerFoundException;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.Charset;

/*
 * 
 * SPA처리를 위한 ControllerAdvice.
 * 요청에 해당하는 Request Mapping이 존재하지 않을 경우 Default로 index.html을 렌더링한다.
 * 
 */

@ControllerAdvice
@Slf4j
public class NotFoundHandler {
//	@Value("${spa.default-file}")
	String defaultFile;
	 
	@ExceptionHandler(NoHandlerFoundException.class)
	public ResponseEntity<String> renderDefaultPage(NoHandlerFoundException ex) {
		String url = ex.getRequestURL();
		if(url.startsWith("/api/")) {
			return ResponseEntity.notFound().build();
		}else {
			try {
				ClassPathResource classPathResource = new ClassPathResource(defaultFile);
				InputStream inputStream = classPathResource.getInputStream();
    				String body = StreamUtils.copyToString(inputStream, Charset.defaultCharset());
			    return ResponseEntity.ok().contentType(MediaType.TEXT_HTML).body(body);
			} catch (IOException e) {
				e.printStackTrace();
				return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("There was an error completing the action.");
			}
		}
	}
	// 500
	@ExceptionHandler({ Exception.class })
	public ResponseEntity<Object> handleAll(final Exception ex) {
		log.info(ex.getClass().getName());
		log.error("error", ex);
		return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(BaseResponseBody.of(500, "Fail"));
	}
}
