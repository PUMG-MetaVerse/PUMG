package com.ghs.pumg.dto.request;


import lombok.*;


@Getter
@Setter
@ToString
@NoArgsConstructor
@AllArgsConstructor
public class UserLoginReq {
    private String userId;
    private String userPassword;

}
