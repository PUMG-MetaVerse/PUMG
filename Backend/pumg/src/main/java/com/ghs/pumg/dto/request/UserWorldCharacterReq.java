package com.ghs.pumg.dto.request;


import lombok.*;


@Getter
@Setter
@ToString
@NoArgsConstructor
@AllArgsConstructor
public class UserWorldCharacterReq {
    private Long userIdx;
    private Long characterIdx;
    private String nickname;

}
