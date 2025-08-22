const ICON_SIZE = 50;

export class Cluster {
  positionPoint: google.maps.Point | null = null;
  position: google.maps.LatLng | null = null;
  features: string[] = [];

  private marker: google.maps.Marker | null = null;
  private listener: google.maps.MapsEventListener | null = null;
  private allPositions: google.maps.LatLng[] = [];

  constructor(public id: string) {}

  addFeature(feature: string, position: google.maps.LatLng, positionPoint: google.maps.Point): void {
    this.features.push(feature);

    // To improve performance let's take the first feature as the center of the cluster
    this.position ??= position;
    this.positionPoint ??= positionPoint;
    this.allPositions.push(position);
  }

  setMarker(googleMap: google.maps.Map): google.maps.Marker {
    if (this.marker != null) {
      return this.marker;
    }

    this.marker = new google.maps.Marker({
      position: this.position,
      label: {
        text: this.features.length.toString(),
        color: 'white',
      },
      icon: {
        url: 'https://maps.google.com/mapfiles/ms/icons/purple.png',
        scaledSize: new google.maps.Size(ICON_SIZE, ICON_SIZE),
        labelOrigin: new google.maps.Point((ICON_SIZE / 2) - 1, ICON_SIZE / 3),
      },
      clickable: true,
      draggable: false,
      visible: true,
      map: googleMap
    });

    this.listener = this.marker.addListener('click', () => {
      const bounds = new google.maps.LatLngBounds();
      for (const position of this.allPositions) {
        bounds.extend(position);
      }

      googleMap.fitBounds(bounds);
      googleMap.setCenter(bounds.getCenter());
    });

    return this.marker;
  }

  remove(): void {
    this.marker?.setMap(null);
    this.marker = null;

    this.listener?.remove();
    this.listener = null;
  }
}