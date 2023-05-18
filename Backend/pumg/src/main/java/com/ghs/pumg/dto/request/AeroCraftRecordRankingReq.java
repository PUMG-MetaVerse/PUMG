package com.ghs.pumg.dto.request;


import lombok.*;


@Getter
@Setter
@ToString
@NoArgsConstructor
@AllArgsConstructor
public class AeroCraftRecordRankingReq {
    private Long userIdx;
    private int score;
    private String clearTime;

}
