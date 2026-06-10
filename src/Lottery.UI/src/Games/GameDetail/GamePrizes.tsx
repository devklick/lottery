import {
  Center,
  Collapse,
  Grid,
  Group,
  Skeleton,
  Stack,
  Text,
  Title,
} from "@mantine/core";
import { Fragment } from "react/jsx-runtime";
import Trophy from "../../components/Trophy/Trophy";
import { useDisclosure } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import PageSection from "../../components/PageSection/PageSection";

interface GamePrizeObject {
  id: string;
  position: number;
  numberMatchCount: number;
}
interface GamePrizesProps {
  prizes: Array<GamePrizeObject>;
  loading: boolean;
  selectionDrawCount: number;
}

export default function GamePrizes({
  prizes,
  loading,
  selectionDrawCount,
}: GamePrizesProps) {
  return (
    <PageSection
      title="Game Prizes"
      subheader="The following prizes are up for grabs"
    >
      <Grid>
        {prizes
          .sort((a, b) => a.position - b.position)
          .map((prize, index) => (
            <Fragment key={`prize-${index}`}>
              <Grid.Col key={`prize-${index}-position`} span={6}>
                <Center>
                  <Trophy loading={loading} position={prize.position} />
                </Center>
              </Grid.Col>
              <Grid.Col key={`prize-${index}-numberMatchCount`} span={6}>
                <Skeleton visible={loading}>
                  <Text>{`${prize.numberMatchCount}/${selectionDrawCount}`}</Text>
                </Skeleton>
              </Grid.Col>
            </Fragment>
          ))}
      </Grid>
    </PageSection>
  );

  /*
  const [opened, { toggle }] = useDisclosure(false);
  return (
    <Stack justify="center" align="center">
      <Group w={"100%"} style={{ alignSelf: "start" }} onClick={toggle}>
        <Title size={"h2"}>Available Prizes</Title>
        {opened ? <IconChevronUp /> : <IconChevronDown />}
      </Group>
      <Collapse expanded={opened}>
        <Text>The following prizes are up for grabs</Text>

      </Collapse>
    </Stack>
  );
  */
}
